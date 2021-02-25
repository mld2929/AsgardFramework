using System;
using System.Text;

using AsgardFramework.DllWrapper;

namespace AsgardFramework.Memory
{
    public sealed class GlobalMemory : KernelDrivenMemoryBase, IGlobalMemory
    {
        private readonly int m_procId;

        #region Constructors

        public GlobalMemory(int processId) : base(Kernel.OpenProcess(Kernel.c_allAccess, true, processId)) {
            m_procId = processId;
        }

        #endregion Constructors

        #region Properties

        public override bool IsInvalid => m_processHandle.IsInvalid;

        #endregion Properties

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

            var memory = new AutoScalingSharedBuffer(Kernel.VirtualAllocEx(m_processHandle, IntPtr.Zero, minSize), m_processHandle, minSize, AllocateShared);
            addWeakHandlerFor(memory);

            return memory;
        }

        public IAutoManagedSharedBuffer AllocateShared(int size) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(GlobalMemory));

            size = calculateSize(size);

            var memory = new AutoManagedSharedBuffer(Kernel.VirtualAllocEx(m_processHandle, IntPtr.Zero, size), m_processHandle, size);
            addWeakHandlerFor(memory);

            return memory;
        }

        public void LoadDll(string path) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(GlobalMemory));

            var loadLibrary = new DllObserver(m_procId, "Kernel32.dll")["LoadLibraryW"];
            var buffer = Allocate(1024);
            buffer.WriteNullTerminatedString(0, path, Encoding.Unicode);
            Kernel.CreateRemoteThread(m_processHandle, 0, 0, loadLibrary, buffer.Start, 0, out _);
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

        private void addWeakHandlerFor(IAutoManagedMemory memory) {
            var weak = new WeakReference<IAutoManagedMemory>(memory);

            Disposing += (_, __) => {
                if (weak.TryGetTarget(out var mem) && !mem.Disposed)
                    mem.Dispose();
            };
        }

        #endregion Methods
    }
}