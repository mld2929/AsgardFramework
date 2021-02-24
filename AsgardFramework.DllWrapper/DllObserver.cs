using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.DllWrapper
{
    public class DllObserver
    {
        #region Fields

        private readonly MODULEENTRY32W m_module;

        #endregion Fields

        #region Constructors

        public DllObserver(int procId, string dllName) {
            var snapshot = Kernel.CreateToolhelp32Snapshot(0x00000008, procId);

            if (snapshot == IntPtr.Zero)
                throw new InvalidOperationException("Can't create toolhelp snapshot");

            var size = Marshal.SizeOf<MODULEENTRY32W>();
            var p = pModule;

            if (!Kernel.Module32FirstW(snapshot, p)) {
                Marshal.FreeHGlobal(p);
                Kernel.CloseHandle(snapshot);

                throw new InvalidOperationException("Can't get first module");
            }

            bool equals;

            do {
                m_module = pToM(p);
                equals = m_module.szModule.Equals(dllName, StringComparison.OrdinalIgnoreCase);
            } while (!equals && Kernel.Module32NextW(snapshot, p));

            Marshal.FreeHGlobal(p);
            Kernel.CloseHandle(snapshot);

            if (!equals)
                throw new InvalidOperationException("Dll not found");
        }

        #endregion Constructors

        #region Properties

        private IntPtr pModule {
            get {
                var size = Marshal.SizeOf<MODULEENTRY32W>();
                var p = Marshal.AllocHGlobal(size);
                Marshal.WriteInt32(p, size);

                return p;
            }
        }

        #endregion Properties

        #region Indexers

        public int this[string procName] {
            get {
                var proc = Kernel.GetProcAddress(m_module.hModule, procName);

                if (proc == 0)
                    throw new InvalidOperationException($"Last error: 0x{Kernel.GetLastError():X}");

                return proc;
            }
        }

        #endregion Indexers

        #region Methods

        private MODULEENTRY32W pToM(IntPtr p) {
            return Marshal.PtrToStructure<MODULEENTRY32W>(p);
        }

        #endregion Methods
    }
}