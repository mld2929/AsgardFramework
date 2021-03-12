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

            internal int[] getResults() {
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

                Frame = buffer.WriteStruct(new object[] {
                    1,
                    CallData
                }, WideFieldsWriteType.Pointer);
            }

            internal ExecutionFrame(IReadOnlyList<(string, object[])> data, IAutoScalingSharedBuffer buffer) {
                CallData = data.Select(d => buffer.WriteStruct(new object[] {
                                   d.Item1,
                                   d.Item2,
                                   0 // reserved for result
                               }, WideFieldsWriteType.Pointer, Encoding.UTF8))
                               .ToList();

                Frame = buffer.WriteStruct(new object[] {
                    CallData.Count,
                    CallData
                }, WideFieldsWriteType.Pointer);
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

        private readonly IAutoManagedMemory m_processQueue;

        private readonly ConcurrentQueue<ExecutionFrame> m_queue = new ConcurrentQueue<ExecutionFrame>();

        private volatile int m_alreadyExecuting;

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
            var func_init_data_list = new List<object[]> {
                new object[] {
                    functionName,
                    functionAddress,
                    functionType,
                    argumentsCount
                }
            };

            var frame = new ExecutionFrame(("RegisterFunctions", new object[] {
                                                   func_init_data_list,
                                                   func_init_data_list.Count
                                               }), m_buffer);

            m_queue.Enqueue(frame);

            executeOrWaitAsync(frame)
                .Wait();
        }

        public void RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions) {
            var func_init_data_list = functions.Select(f => new object[] {
                                                   f.functionName,
                                                   f.functionAddress,
                                                   f.functionType,
                                                   f.argumentsCount
                                               })
                                               .ToList();

            var frame = new ExecutionFrame(("RegisterFunctions", new object[] {
                                                   func_init_data_list,
                                                   func_init_data_list.Count
                                               }), m_buffer);

            m_queue.Enqueue(frame);

            executeOrWaitAsync(frame)
                .Wait();
        }

        private async Task<int> executeAsync(string functionName, params object[] args) {
            var frame = new ExecutionFrame((functionName, args), m_buffer);
            m_queue.Enqueue(frame);

            return (await executeOrWaitAsync(frame)
                        .ConfigureAwait(false))[0];
        }

        private async Task<int[]> executeOrWaitAsync(ExecutionFrame frame) {
            while (!frame.Executed) {
                if (Interlocked.CompareExchange(ref m_alreadyExecuting, 1, 0) != 0)
                    continue;

                var dequeued = await flushQueueAsync()
                                   .ConfigureAwait(false);

                m_event.ResetEvent();

                await m_event.WaitForSignalAsync()
                             .ConfigureAwait(false);

                dequeued.ForEach(fr => fr.Executed = true);
                Interlocked.Decrement(ref m_alreadyExecuting);
            }

            return frame.getResults();
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
}