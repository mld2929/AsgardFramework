using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    public class GlobalMemory : IGlobalMemory
    {
        private readonly SafeHandle m_handle;
        public GlobalMemory(int processId)
        {
            m_handle = Kernel.OpenProcess(Kernel.c_allAccess, false, processId);
        }

        public byte[] Read(int address, int count)
        {
            var buffer = new byte[count];
            Kernel.ReadProcessMemory(m_handle, address, buffer, count, out var _);
            return buffer;
        }

        public void Write(int address, byte[] data) => Kernel.WriteProcessMemory(m_handle, address, data, data.Length, out var _);

        public IAutoManagedMemory Allocate(int size)
        {
            size += size % 1024 == 0 ? 0 : 1024 - size % 1024;
            return new AutoManagedMemory(Kernel.VirtualAllocEx(m_handle, IntPtr.Zero, size), m_handle, size);
        }

        public T Read<T>(int offset) where T : class, new()
        {
            var obj = new T();
            Kernel.ReadProcessMemory(m_handle, offset, obj, Marshal.SizeOf(obj), out var _);
            return obj;
        }
        public void Write<T>(int offset, T data) where T : class, new() => Kernel.WriteProcessMemory(m_handle, offset, data, Marshal.SizeOf(data), out var _);
    }
}
