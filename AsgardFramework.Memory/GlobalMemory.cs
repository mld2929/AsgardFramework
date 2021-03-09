using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public sealed class GlobalMemory : KernelDrivenMemoryBase, IGlobalMemory
    {
        #region Constructors

        public GlobalMemory(int processId) : base(Kernel.OpenProcess(Kernel.c_allAccess, true, processId), IntPtr.Zero) {
            m_procId = processId;
            m_procKernel = DllWrapper.GetKernel(m_procId, m_processHandle, AllocateAutoScalingShared(4096));
        }

        #endregion Constructors

        #region Properties

        public override bool IsInvalid => m_processHandle.IsInvalid;

        #endregion Properties

        #region Fields

        private const string c_executorCore = "AsgardFramework.Core.dll";

        private readonly int m_procId;

        private readonly DllWrapper m_procKernel;

        private IMainThreadExecutor m_mainThreadExecutor;

        #endregion Fields

        #region Methods

        public IAutoManagedMemory Allocate(int size) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(GlobalMemory));

            size = calculateSize(size);
            var memory = new AutoManagedMemory(Kernel.VirtualAllocEx(m_processHandle, IntPtr.Zero, size), m_processHandle, size);
            addWeakHandlerFor(memory);

            return memory;
        }

        public IAutoScalingSharedBuffer AllocateAutoScalingShared(int minSize) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(GlobalMemory));

            minSize = calculateSize(minSize);

            var memory = new AutoScalingSharedBuffer(Kernel.VirtualAllocEx(m_processHandle, IntPtr.Zero, minSize), m_processHandle, minSize, allocateAutoManagedSharedBuffer);
            addWeakHandlerFor(memory);

            return memory;
        }

        public IAutoManagedSharedBuffer AllocateShared(int size) {
            return allocateAutoManagedSharedBuffer(size);
        }

        public IMainThreadExecutor GetMainThreadExecutor() {
            return m_mainThreadExecutor ??= new EndSceneHookExecutor(Allocate(0x80000), LoadDll(c_executorCore));
        }

        public IDll LoadDll(string path) {
            var fullPath = Path.GetFullPath(path);

            if (fullPath == null || !File.Exists(fullPath))
                throw new DllNotFoundException(path);

            m_procKernel["LoadLibraryW", true, Encoding.Unicode](fullPath);

            var dll = new DllWrapper(Path.GetFileName(path), m_processHandle, m_procId, m_procKernel, AllocateAutoScalingShared(4096));
            addWeakHandlerFor(dll);

            return dll;
        }

        public async Task<IDll> LoadDllAsync(string path) {
            var fullPath = Path.GetFullPath(path);

            if (fullPath == null || !File.Exists(fullPath))
                throw new DllNotFoundException(path);

            await m_procKernel["LoadLibraryW", true, Encoding.Unicode, fullPath]
                .ConfigureAwait(false);

            var dll = new DllWrapper(Path.GetFileName(path), m_processHandle, m_procId, m_procKernel, AllocateAutoScalingShared(4096));
            addWeakHandlerFor(dll);

            return dll;
        }

        protected override bool ReleaseHandle() {
            m_processHandle.Dispose();

            return m_processHandle.IsClosed;
        }

        private static int calculateSize(int requested) {
            var mod = requested % 1024;
            requested += mod == 0 ? 0 : 1024 - mod;

            return requested;
        }

        private void addWeakHandlerFor(IDisposable memory) {
            var weak = new WeakReference<IDisposable>(memory);

            Disposing += (_, __) => {
                if (!weak.TryGetTarget(out var disp))
                    return;

                if (disp is IAutoManagedMemory {
                    Disposed: true
                })
                    return;

                disp.Dispose();
            };
        }

        private AutoManagedSharedBuffer allocateAutoManagedSharedBuffer(int size) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(GlobalMemory));

            size = calculateSize(size);

            var memory = new AutoManagedSharedBuffer(Kernel.VirtualAllocEx(m_processHandle, IntPtr.Zero, size), m_processHandle, size);
            addWeakHandlerFor(memory);

            return memory;
        }

        #endregion Methods
    }
}