using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    public class GlobalMemory : IGlobalMemory
    {
        #region Fields

        private readonly SafeHandle m_handle;

        #endregion Fields

        #region Constructors

        public GlobalMemory(int processId) {
            m_handle = Kernel.OpenProcess(Kernel.c_allAccess, true, processId);

            if (m_handle.IsInvalid)
                throw new InvalidOperationException();

            var weakThis = new WeakReference<GlobalMemory>(this);

            AppDomain.CurrentDomain.ProcessExit += (_, __) => {
                if (weakThis.TryGetTarget(out var @this))
                    @this.m_handle.Dispose();
            };
        }

        #endregion Constructors

        #region Methods

        public IAutoManagedMemory Allocate(int size) {
            size += size % 1024 == 0 ? 0 : 1024 - size % 1024;

            return new AutoManagedMemory(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, size), m_handle, size);
        }

        public IAutoManagedSharedBuffer AllocateAutoScalingShared(int minSize) {
            minSize += minSize % 1024 == 0 ? 0 : 1024 - minSize % 1024;

            return new AutoScalingSharedBuffer(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, minSize), m_handle, minSize, AllocateShared);
        }

        public IAutoManagedSharedBuffer AllocateShared(int size) {
            size += size % 1024 == 0 ? 0 : 1024 - size % 1024;

            return new AutoManagedSharedBuffer(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, size), m_handle, size);
        }

        public byte[] Read(int offset, int count) {
            var buffer = new byte[count];
            Kernel.ReadProcessMemory(m_handle, offset, buffer, count, out var _);

            return buffer;
        }

        public T Read<T>(int offset) where T : new() {
            var bytes = new byte[Marshal.SizeOf<T>()];
            Kernel.ReadProcessMemory(m_handle, offset, bytes, bytes.Length, out var _);

            unsafe {
                fixed (byte* buffer = bytes) {
                    return Marshal.PtrToStructure<T>((IntPtr)buffer);
                }
            }
        }

        public void Write(int offset, byte[] data) {
            Kernel.WriteProcessMemory(m_handle, offset, data, data.Length, out var _);
        }

        public void Write<T>(int offset, T data) where T : new() {
            var bytes = new byte[Marshal.SizeOf<T>()];

            unsafe {
                fixed (byte* buffer = bytes) {
                    Marshal.StructureToPtr(data, (IntPtr)buffer, true);
                }
            }

            Kernel.WriteProcessMemory(m_handle, offset, bytes, bytes.Length, out var _);
        }

        #endregion Methods
    }
}