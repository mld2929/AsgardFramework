using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public enum FunctionCallType
    {
        /// <summary>
        ///     Caller cleans up stack, arguments pushed to stack
        /// </summary>
        Cdecl,

        /// <summary>
        ///     Callee cleans up stack, arguments pushed to stack
        /// </summary>
        Stdcall,

        /// <summary>
        ///     Caller cleans up stack, first argument (<see langword="this" />) moved to ecx, other arguments pushed to stack
        /// </summary>
        Thiscall,

        /// <summary>
        ///     Caller cleans up stack, first argument (<see langword="this" />) moved to ecx, second argument is index of virtual
        ///     function, other arguments pushed to stack
        /// </summary>
        Virtualcall
    }

    internal sealed class EndSceneHookExecutor : IMainThreadExecutor
    {
        #region Constructors

        internal EndSceneHookExecutor(IAutoManagedMemory queue, IDll core) {
            m_queue = queue;
            m_core = core;
            m_event = new InterprocessManualResetEventSlim((IntPtr)m_core[c_init, false, Encoding.UTF8](m_queue), false);
        }

        #endregion Constructors

        #region Fields

        private const string c_init = "InitInteraction";

        private const string c_register = "RegisterFunctions";

        private readonly IDll m_core;

        private readonly InterprocessManualResetEventSlim m_event;

        private readonly IAutoManagedMemory m_queue;

        #endregion Fields

        #region Indexers

        public Task<int> this[string functionName, params object[] arguments] => throw new NotImplementedException();

        public RemoteMainThreadFunction this[string functionName] => throw new NotImplementedException();

        #endregion Indexers

        #region Methods

        public RemoteMainThreadFunction RegisterFunction(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount) {
            var registrationData = new object[] {
                functionName,
                functionType,
                functionAddress,
                argumentsCount
            };

            m_core[c_register, false, Encoding.UTF8](new object[] {
                registrationData
            }, 1);

            return this[functionName];
        }

        public IReadOnlyList<RemoteMainThreadFunction> RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions) {
            var registrationData = functions.Select(descriptor => new object[] {
                                                descriptor.functionName,
                                                descriptor.functionType,
                                                descriptor.functionAddress,
                                                descriptor.argumentsCount
                                            })
                                            .ToArray();

            m_core[c_register, false, Encoding.UTF8](registrationData, registrationData.Length);

            return functions.Select(descriptor => this[descriptor.functionName])
                            .ToList();
        }

        #endregion Methods
    }
}