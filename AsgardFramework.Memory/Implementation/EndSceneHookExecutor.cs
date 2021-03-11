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
            m_core = core;
            m_event = m_core.InitializeInteraction(m_processQueue);
        }

        #endregion Constructors

        #region Classes

        private class ExecutionFrame
        {
            #region Methods

            internal async Task<int[]> waitForResults() {
                while (!Executed)
                    await Task.Yield();

                var results = CallData.Select(data => data.Read<int>(8))
                                      .ToArray();

                CallData.ForEach(d => d.Dispose());
                Frame.Dispose();

                return results;
            }

            #endregion Methods

            #region Fields

            internal readonly IAutoManagedMemory Frame;
            internal bool Executed;
            private readonly List<IAutoManagedMemory> CallData;

            #endregion Fields

            #region Constructors

            internal ExecutionFrame((string, object[]) data, IAutoScalingSharedBuffer buffer) {
                CallData = new List<IAutoManagedMemory> {
                    buffer.WriteStruct(new object[] {
                        data.Item1,
                        data.Item2,
                        0 // reserved for result
                    }, WideFieldsWriteType.Pointer, Encoding.UTF8)
                };

                Frame = buffer.WriteStruct(CallData);
            }

            internal ExecutionFrame(IReadOnlyList<(string, object[])> data, IAutoScalingSharedBuffer buffer) {
                CallData = data.Select(d => buffer.WriteStruct(new object[] {
                                   d.Item1,
                                   d.Item2,
                                   0 // reserved for result
                               }, WideFieldsWriteType.Pointer, Encoding.UTF8))
                               .ToList();

                Frame = buffer.WriteStruct(CallData);
            }

            #endregion Constructors
        }

        #endregion Classes

        #region Fields

        private const int c_callDataSize = 4;

        private const int c_queueCapacity = 2048;

        private const int c_queueSize = c_queueCapacity * c_callDataSize;

        private readonly IAutoScalingSharedBuffer m_buffer;

        private readonly ICoreDll m_core;

        private readonly InterprocessManualResetEventSlim m_event;

        private readonly OptionalLock m_executionLock = new OptionalLock();

        private readonly IAutoManagedMemory m_processQueue;

        private readonly ConcurrentQueue<ExecutionFrame> m_queue = new ConcurrentQueue<ExecutionFrame>();

        private readonly OptionalLock m_registrationLock = new OptionalLock();

        #endregion Fields

        #region Indexers

        public RemoteMainThreadAsyncFunction this[string functionName] => args => executeAsync(functionName, args);

        public RemoteMainThreadFunction this[string functionName, int timeout] =>
            args => executeAsync(functionName, args)
                .SyncWaitResult(timeout);

        public RemoteMainThreadAsyncFunctionsChain this[IEnumerable<string> functions] => args => executeSequentiallyAsync(functions.Zip(args));

        #endregion Indexers

        #region Methods

        public void RegisterFunction(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount) {
            m_registrationLock.StartLockedAccess();

            m_core.RegisterFunction(functionName, functionType, functionAddress, argumentsCount);
            m_registrationLock.EndLockedAccess();
        }

        public void RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions) {
            m_registrationLock.StartLockedAccess();

            m_core.RegisterFunctions(functions);
            m_registrationLock.EndLockedAccess();
        }

        private async Task<int> executeAsync(string functionName, params object[] args) {
            var frame = new ExecutionFrame((functionName, args), m_buffer);
            m_queue.Enqueue(frame);

            return (await executeOrWaitAsync(frame)
                        .ConfigureAwait(false))[0];
        }

        private async Task<int[]> executeOrWaitAsync(ExecutionFrame frame) {
            await m_registrationLock.StartAccessAsync()
                                    .ConfigureAwait(false);

            if (!await m_executionLock.TryStartLockedAccessAsync()
                                      .ConfigureAwait(false)) {
                m_registrationLock.EndAccess();

                return await frame.waitForResults()
                                  .ConfigureAwait(false);
            }

            var dequeued = await flushQueueAsync()
                               .ConfigureAwait(false);

            m_event.ResetEvent();

            await m_event.WaitForSignalAsync()
                         .ConfigureAwait(false);

            m_executionLock.EndLockedAccess();
            m_registrationLock.EndAccess();

            dequeued.ForEach(fr => fr.Executed = true);

            return await frame.waitForResults()
                              .ConfigureAwait(false);
        }

        private Task<int[]> executeSequentiallyAsync(IEnumerable<(string functionName, object[] args)> functions) {
            var frame = new ExecutionFrame(functions.ToList(), m_buffer);
            m_queue.Enqueue(frame);

            return executeOrWaitAsync(frame);
        }

        private async Task<List<ExecutionFrame>> flushQueueAsync() {
            await m_event.WaitForSignalAsync()
                         .ConfigureAwait(false);

            m_processQueue.Fill(0);
            var dequeued = new List<ExecutionFrame>();

            for (var i = 0; i < c_queueCapacity; i++) {
                if (!m_queue.TryDequeue(out var frame))
                    break;

                dequeued.Add(frame);
            }

            m_processQueue.Write(0, dequeued.Select(f => f.Frame));

            return dequeued;
        }

        #endregion Methods
    }
    // doesn't work lol, i got ~50 tasks entered locked section
    internal class OptionalLock
    {
        #region Fields

        private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1, 1);
        private volatile int m_accessers;

        private volatile int m_lockedAccessCount;

        #endregion Fields

        #region Methods

        internal void EndAccess() {
            Interlocked.Decrement(ref m_accessers);
        }

        internal void EndLockedAccess() {
            m_semaphore.Release();
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

            m_semaphore.Wait();
        }

        internal async Task StartLockedAccessAsync() {
            Interlocked.Increment(ref m_lockedAccessCount);

            while (m_accessers != 0)
                await Task.Yield();

            await m_semaphore.WaitAsync()
                             .ConfigureAwait(false);
        }

        internal async Task<bool> TryStartLockedAccessAsync() {
            Interlocked.Increment(ref m_lockedAccessCount);

            var entered = await m_semaphore.WaitAsync(0)
                                           .ConfigureAwait(false);

            if (!entered)
                Interlocked.Decrement(ref m_lockedAccessCount);

            return entered;
        }

        #endregion Methods
    }
}