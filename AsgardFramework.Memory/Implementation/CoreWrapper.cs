using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AsgardFramework.Memory.Implementation
{
    internal class CoreWrapper : DllWrapper, ICoreDll
    {
        #region Constructors

        public CoreWrapper(SafeHandle hProcess, int processId, DllWrapper kernel, IAutoScalingSharedBuffer buffer) : base(c_coreName, hProcess, processId, kernel, buffer) { }

        #endregion Constructors

        #region Fields

        internal const string c_coreName = "AsgardFramework.Core.dll";

        private const string c_init = "InitInteraction";

        private const string c_register = "RegisterFunctions";

        #endregion Fields

        #region Methods

        public InterprocessManualResetEventSlim InitializeInteraction(IAutoManagedMemory queue) {
            if (!Kernel.DuplicateHandle(m_process, (IntPtr)this[c_init, false](queue), Process.GetCurrentProcess()
                                                                                              .Handle, out var duplicated))
                throw new InvalidOperationException($"Can't duplicate handle (error: 0x{Kernel.GetLastError():X})");

            return new InterprocessManualResetEventSlim(duplicated, true);
        }

        public void RegisterFunction(string name, FunctionCallType type, int address, int argumentsCount) {
            var registrationData = new object[] {
                name,
                address,
                type,
                argumentsCount
            };

            this[c_register, false, Encoding.UTF8](new object[] {
                registrationData
            }, 1);
        }

        public void RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions) {
            var registrationData = functions.Select(descriptor => new object[] {
                                                descriptor.functionName,
                                                descriptor.functionAddress,
                                                descriptor.functionType,
                                                descriptor.argumentsCount
                                            })
                                            .ToArray();

            this[c_register, false, Encoding.UTF8](registrationData, registrationData.Length);
        }

        #endregion Methods
    }
}