using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    public class GlobalMemory : IGlobalMemory
    {
        private readonly SafeHandle m_handle;
        public GlobalMemory(int processId) {
            m_handle = Kernel.OpenProcess(Kernel.c_allAccess, false, processId);
            if (m_handle.IsInvalid) {
                throw new InvalidOperationException();
            }
        }

        public byte[] Read(int address, int count) {
            var buffer = new byte[count];
            Kernel.ReadProcessMemory(m_handle, address, buffer, count, out var _);
            return buffer;
        }

        public void Write(int address, byte[] data) {
            Kernel.WriteProcessMemory(m_handle, address, data, data.Length, out var _);
        }

        public IAutoManagedMemory Allocate(int size) {
            size += size % 1024 == 0 ? 0 : 1024 - size % 1024;
            return new AutoManagedMemory(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, size), m_handle, size);
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
        public void Write<T>(int offset, T data) where T : new() {
            var bytes = new byte[Marshal.SizeOf<T>()];
            unsafe {
                fixed (byte* buffer = bytes) {
                    Marshal.StructureToPtr(data, (IntPtr)buffer, true);
                }
            }
            Kernel.WriteProcessMemory(m_handle, offset, bytes, bytes.Length, out var _);
        }

        public IAutoManagedSharedBuffer AllocateShared(int size) {
            size += size % 1024 == 0 ? 0 : 1024 - size % 1024;
            return new AutoManagedSharedBuffer(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, size), m_handle, size);
        }

        public IAutoManagedSharedBuffer AllocateAutoScalingShared(int size) {
            size += size % 1024 == 0 ? 0 : 1024 - size % 1024;
            return new AutoScalingSharedBuffer(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, size), m_handle, size, AllocateShared);
        }

    }
}
