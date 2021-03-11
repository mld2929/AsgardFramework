using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsgardFramework.Memory.Implementation
{
    internal static class TaskHelper
    {
        #region Methods

        public static T SyncWaitResult<T>(this Task<T> task, int timeout) {
            task.Wait(timeout);

            if (!task.IsCompleted)
                throw new TimeoutException();

            return task.Result;
        }

        #endregion Methods
    }

    internal sealed class EndSceneHookExecutor : IMainThreadExecutor
    {
        #region Constructors

        internal EndSceneHookExecutor(IAutoScalingSharedBuffer buffer, ICoreDll core) {
            m_buffer = buffer;
            m_processQueue = m_buffer.Reserve(c_queueSize);
            m_results = m_buffer.Reserve(c_queueCapacity * 4);
            m_core = core;
            m_event = m_core.InitializeInteraction(m_processQueue);
        }

        #endregion Constructors

        #region Classes

        private class ExecutionFrame
        {
            #region Fields

            internal readonly IReadOnlyList<(string, object[])> callData;
            internal readonly int callsCount;
            internal int resultsIndex = -1;

            #endregion Fields

            #region Constructors

            internal ExecutionFrame((string, object[]) data) {
                callsCount = 1;

                callData = new[] {
                    data
                };
            }

            internal ExecutionFrame(IEnumerable<(string, object[])> data) {
                callData = data.ToList();
                callsCount = callData.Count;

                if (callsCount > c_queueCapacity)
                    throw new ArgumentException($"Too many functions in one frame ({callsCount} vs {c_queueCapacity})");
            }

            #endregion Constructors

            #region Methods

            internal int[] GetMultipleResults(IAutoManagedMemory resultsBuffer, ref int readers) {
                var result = resultsBuffer.Read<int>(resultsIndex * 4, callsCount);
                Interlocked.Add(ref readers, -callsCount);

                return result;
            }

            internal int GetResult(IAutoManagedMemory resultBuffer, ref int readers) {
                var result = resultBuffer.Read<int>(resultsIndex * 4);
                Interlocked.Decrement(ref readers);

                return result;
            }

            #endregion Methods
        }

        #endregion Classes

        #region Fields

        private const int c_callDataSize = 4;

        private const int c_queueCapacity = 2048;

        private const int c_queueSize = c_queueCapacity * c_callDataSize;

        private readonly IAutoScalingSharedBuffer m_buffer;

        private readonly ICoreDll m_core;

        private readonly InterprocessManualResetEventSlim m_event;

        private readonly ExecutionLock m_lock = new ExecutionLock();

        private readonly IAutoManagedMemory m_processQueue;

        private readonly ConcurrentQueue<ExecutionFrame> m_queue = new ConcurrentQueue<ExecutionFrame>();

        private readonly IAutoManagedMemory m_results;

        private int m_readers;

        #endregion Fields

        #region Indexers

        public RemoteMainThreadAsyncFunction this[string functionName] => args => executeAsync(functionName, args);

        public RemoteMainThreadFunction this[string functionName, int timeout] =>
            args => executeAsync(functionName, args)
                .SyncWaitResult(timeout);

        public RemoteMainThreadAsyncFunctionsChain this[IEnumerable<string> functions] => args => executeSequentiallyAsync(functions.Zip(args));

        #endregion Indexers

        #region Methods

        public RemoteMainThreadFunction RegisterFunction(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount) {
            m_lock.StartLockedAccess();

            m_core.RegisterFunction(functionName, functionType, functionAddress, argumentsCount);
            m_lock.EndLockedAccess();

            return this[functionName, Timeout.Infinite];
        }

        public IReadOnlyList<RemoteMainThreadFunction> RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions) {
            m_lock.StartLockedAccess();

            m_core.RegisterFunctions(functions);
            m_lock.EndLockedAccess();

            return functions.Select(descriptor => this[descriptor.functionName, Timeout.Infinite])
                            .ToList();
        }

        private async Task<int> executeAsync(string functionName, params object[] args) {
            var frame = new ExecutionFrame((functionName, args));
            m_queue.Enqueue(frame);

            await executeOrWaitAsync(frame)
                .ConfigureAwait(false);

            return frame.GetResult(m_results, ref m_readers);
        }

        private async Task executeOrWaitAsync(ExecutionFrame frame) {
            while (frame.resultsIndex == -1) {
                while (m_readers != 0)
                    await Task.Yield();

                if (await m_lock.TryStartLockedAccessAsync()
                                .ConfigureAwait(false)) {
                    var pointers = await flushQueue()
                                       .ConfigureAwait(false);

                    m_event.ResetEvent();

                    await m_event.WaitForSignalAsync()
                                 .ConfigureAwait(false);

                    foreach (var ptr in pointers)
                        ptr.Dispose();

                    m_lock.EndLockedAccess();
                } else {
                    await m_lock.StartAccessAsync()
                                .ConfigureAwait(false);

                    m_lock.EndAccess();
                }
            }
        }

        private async Task<int[]> executeSequentiallyAsync(IEnumerable<(string functionName, object[] args)> functions) {
            var frame = new ExecutionFrame(functions);
            m_queue.Enqueue(frame);

            await executeOrWaitAsync(frame)
                .ConfigureAwait(false);

            return frame.GetMultipleResults(m_results, ref m_readers);
        }

        private async Task<IReadOnlyList<IAutoManagedMemory>> flushQueue() {
            await m_event.WaitForSignalAsync()
                         .ConfigureAwait(false);

            await m_lock.StartLockedAccessAsync()
                        .ConfigureAwait(false);

            m_processQueue.Fill(0);
            var pointers = new List<IAutoManagedMemory>();

            for (var dequeued = 0; dequeued < c_queueCapacity;) {
                if (!m_queue.TryPeek(out var function))
                    break;

                if (function.callsCount > c_queueCapacity - dequeued)
                    break;

                m_readers++;
                m_queue.TryDequeue(out _);
                function.resultsIndex = dequeued;

                for (int callDataIndex = 0, offset = 0; callDataIndex < function.callsCount; offset = (dequeued + callDataIndex) * 4, callDataIndex++, dequeued++) {
                    var (name, args) = function.callData.ElementAt(callDataIndex);

                    var pointer = m_buffer.WriteStruct(new object[] {
                        name,
                        args,
                        m_results + offset
                    }, WideFieldsWriteType.Pointer, Encoding.UTF8);

                    m_processQueue.Write(offset, pointer);
                    pointers.Add(pointer);
                }
            }

            m_lock.EndLockedAccess();

            return pointers;
        }

        #endregion Methods
    }

    // todo: test and rewrite if needed
    internal class ExecutionLock
    {
        #region Fields

        //private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(0, 1);
        private volatile int m_accessers;

        private volatile int m_lockedAccessCount;

        #endregion Fields

        #region Methods

        internal void EndAccess() {
            Interlocked.Decrement(ref m_accessers);
        }

        internal void EndLockedAccess() {
            //m_semaphore.Release();
            Interlocked.Decrement(ref m_lockedAccessCount);
        }

        internal async Task StartAccessAsync() {
            while (m_lockedAccessCount != 0)
                await Task.Yield();

            Interlocked.Increment(ref m_accessers);
        }

        internal void StartLockedAccess() {
            Interlocked.Increment(ref m_lockedAccessCount);

            while (m_accessers != 0)
                ;

            //m_semaphore.Wait();
        }

        internal async Task StartLockedAccessAsync() {
            Interlocked.Increment(ref m_lockedAccessCount);

            while (m_accessers != 0)
                await Task.Yield();

            //await m_semaphore.WaitAsync().ConfigureAwait(true);
        }

        internal async Task<bool> TryStartLockedAccessAsync() {
            Interlocked.Increment(ref m_lockedAccessCount);

            //var entered = await m_semaphore.WaitAsync(0)
            //.ConfigureAwait(true);

            //if (!entered)
            //Interlocked.Decrement(ref m_lockedAccessCount);

            //return entered;
            return true;
        }

        #endregion Methods
    }
}