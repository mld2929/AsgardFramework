using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AsgardFramework.Memory.Implementation
{
    internal class DllWrapper : SafeHandle, IDll
    {
        #region Fields

        protected readonly int m_pid;
        protected readonly SafeHandle m_process;
        private const int c_padding = 0x10;

        // this is x86 assembly code, it allows to push any arguments to callee (encoded in UTF16-LE)
        private const string c_unwrapper = "ﾉ襕菥ࣅ疋謀ࡎ೫䚋謌蠄䖉＀u礁華Ѿ琀）褖ࡆ쉝\0䚋섈ˠ䖉＀褖ࡆ攃崀Â";

        private readonly IAutoScalingSharedBuffer m_buffer;
        private readonly object m_cacheLock = new object();
        private readonly DllWrapper m_kernel;
        private readonly Dictionary<string, int> m_procsCache = new Dictionary<string, int>();
        private readonly IAutoManagedMemory m_unwrapper;

        #endregion Fields

        #region Constructors

        public DllWrapper(string name, SafeHandle hProcess, int processId, DllWrapper kernel, IAutoScalingSharedBuffer buffer) : base(IntPtr.Zero, true) {
            m_process = hProcess;
            m_kernel = kernel;
            m_buffer = buffer;
            m_unwrapper = m_kernel.m_unwrapper;
            m_pid = processId;
            SetHandle(getHandle(processId, name));
        }

        private DllWrapper(int processId, SafeHandle hProcess, IAutoScalingSharedBuffer buffer) : base(IntPtr.Zero, false) {
            m_process = hProcess;
            m_buffer = buffer;
            m_unwrapper = buffer.Reserve(c_unwrapper.Length * 4 + c_padding);
            m_unwrapper.WriteNullTerminatedString(c_padding, c_unwrapper, Encoding.Unicode);
            m_pid = processId;
            SetHandle(getHandle(processId, "Kernel32.dll"));
        }

        #endregion Constructors

        #region Properties

        public override bool IsInvalid => handle == IntPtr.Zero;
        public string Name { get; private set; }

        #endregion Properties

        #region Indexers

        public RemoteFunction this[string name, bool isStd, Encoding encoding = default] => args => callSync(name, isStd, encoding ?? Encoding.Unicode, args);

        public RemoteAsyncFunction this[bool isStd, string name, Encoding encoding = default] => args => callAsync(name, isStd, encoding ?? Encoding.Unicode, args);

        #endregion Indexers

        #region Methods

        public static DllWrapper GetKernel(int processId, SafeHandle hProcess, IAutoScalingSharedBuffer buffer) {
            return new DllWrapper(processId, hProcess, buffer);
        }

        protected override bool ReleaseHandle() {
            return m_kernel["FreeLibrary", true]((int)handle) != 0;
        }

        private async Task<int> callAsync(string name, bool isStd, Encoding encoding, params object[] args) {
            if (name == "LoadLibraryW")
                return await LoadLibraryWAsync((string)args[0])
                           .ConfigureAwait(false);

            var unmanagedArgs = writeArgs(getProcAddress(name), isStd, encoding, args);
            var thread = callProc(unmanagedArgs);

            await thread.WaitForSignalAsync()
                        .ConfigureAwait(false);

            return getResultAndFreeMemory(thread, unmanagedArgs);
        }

        private SafeWaitHandleSlim callProc(IAutoManagedMemory unwrapperArg) {
            var thread = new SafeWaitHandleSlim(Kernel.CreateRemoteThread(m_process, 0, 0, m_unwrapper + c_padding, unwrapperArg.Start, 0, out _), true);

            if (thread.DangerousGetHandle() == IntPtr.Zero)
                throw new InvalidOperationException($"Error: 0x{Kernel.GetLastError():X}");

            return thread;
        }

        private int callSync(string name, bool isStd, Encoding encoding, params object[] args) {
            if (name == "LoadLibraryW")
                return LoadLibraryW((string)args[0]);

            var unmanagedArgs = writeArgs(getProcAddress(name), isStd, encoding, args);

            return getResultAndFreeMemory(callProc(unmanagedArgs), unmanagedArgs);
        }

        private IntPtr getHandle(int processId, string dllName) {
            var snapshot = Kernel.CreateToolhelp32Snapshot(0x00000008, processId);

            static IntPtr allocModule() {
                var size = Marshal.SizeOf<MODULEENTRY32W>();
                var p = Marshal.AllocHGlobal(size);
                Marshal.WriteInt32(p, size);

                return p;
            }

            var pModule = allocModule();
            MODULEENTRY32W module = null;
            var equals = false;
            var name = Path.GetFileName(dllName);
            while (!equals && Kernel.Module32NextW(snapshot, pModule)) {
                module = Marshal.PtrToStructure<MODULEENTRY32W>(pModule);
                equals = module?.szModule.Equals(name, StringComparison.OrdinalIgnoreCase) ?? false;
            }

            if (!equals)
                throw new ArgumentException($"DLL \"{name}\" ({dllName}) not found", nameof(dllName));
            Marshal.FreeHGlobal(pModule);
            Kernel.CloseHandle(snapshot);

            Name = module.szModule;


            return (IntPtr)module.hModule;
        }

        private int getProcAddress(string procName) {
            lock (m_cacheLock) {
                if (m_procsCache.TryGetValue(procName, out var proc))
                    return proc;

                proc = m_kernel?["GetProcAddress", true, Encoding.ASCII](handle, procName) ?? Kernel.GetProcAddress(handle, procName);

                if (proc == 0)
                    throw new ArgumentException($"{Name}::{procName} not found (0x{Kernel.GetLastError():X})", nameof(procName));

                m_procsCache.Add(procName, proc);

                return proc;
            }
        }

        private int getResultAndFreeMemory(SafeWaitHandleSlim thread, IAutoManagedMemory unwrapperArg) {
            thread.WaitForSingleObject(-1);

            if (!Kernel.GetExitCodeThread(thread.DangerousGetHandle(), out var code))
                throw new ArgumentException($"Can't get result from given handle (Error: 0x{Kernel.GetLastError()}, exit code: {code})", nameof(thread));

            var result = unwrapperArg.Read<int>(8);
            unwrapperArg.Dispose();

            return result;
        }

        private int LoadLibraryW(string name) {
            var address = getProcAddress("LoadLibraryW");
            var pName = m_buffer.WriteString(name, Encoding.Unicode);
            var thread = new SafeWaitHandleSlim(Kernel.CreateRemoteThread(m_process, 0, 0, address, pName.Start, 0, out _), true);
            thread.WaitForSingleObject(-1);
            var hModule = getHandle(m_pid, name);
            pName.Dispose();
            thread.Dispose();

            return (int)hModule;
        }

        private async Task<int> LoadLibraryWAsync(string name) {
            var address = getProcAddress("LoadLibraryW");
            var pName = m_buffer.WriteString(name, Encoding.Unicode);
            var thread = new SafeWaitHandleSlim(Kernel.CreateRemoteThread(m_process, 0, 0, address, pName.Start, 0, out _), true);

            await thread.WaitForSignalAsync()
                        .ConfigureAwait(false);

            var hModule = getHandle(m_pid, name);
            pName.Dispose();
            thread.Dispose();

            return (int)hModule;
        }

        private IAutoManagedMemory writeArgs(int procAddress, bool isStd, Encoding encoding, params object[] args) {
            return (args?.Length ?? 0) switch {
                0 => m_buffer.WriteStruct(new object[] {
                    procAddress,
                    isStd,
                    0,
                    0
                }, WideFieldsWriteType.Pointer, encoding),
                1 => m_buffer.WriteStruct(new[] {
                    procAddress,
                    isStd,
                    1,
                    args![0]
                }, WideFieldsWriteType.Pointer, encoding),
                _ => m_buffer.WriteStruct(new object[] {
                    procAddress,
                    isStd,
                    args.Length,
                    args
                }, WideFieldsWriteType.Pointer, encoding)
            };
        }

        #endregion Methods
    }
}