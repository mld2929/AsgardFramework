using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    internal class AutoManagedMemory : SafeHandleZeroOrMinusOneIsInvalid, IAutoManagedMemory
    {
        private readonly SafeHandle m_processHandle;
        internal AutoManagedMemory(IntPtr address, SafeHandle processHandle, int size) : base(true) {
            handle = address;
            m_processHandle = processHandle;
            Size = size;
        }

        public int Size { get; private set; }

        public int Start => handle.ToInt32();

        public byte[] Read(int offset, int count) {
            if (count + offset > Size) {
                throw new ArgumentOutOfRangeException();
            }

            var result = new byte[count];
            Kernel.ReadProcessMemory(m_processHandle, handle + offset, result, count, out var _);
            return result;
        }
        public void Write(int offset, byte[] data) {
            if (data.Length + offset > Size) {
                throw new ArgumentOutOfRangeException();
            }

            Kernel.WriteProcessMemory(m_processHandle, handle + offset, data, data.Length, out var _);
        }

        public T Read<T>(int offset) where T : class, new() {
            var size = Marshal.SizeOf<T>();
            if (size + offset > Size) {
                throw new ArgumentOutOfRangeException();
            }
            var buffer = Marshal.AllocHGlobal(size);
            Kernel.ReadProcessMemory(m_processHandle, offset, buffer, size, out var _);
            var obj = Marshal.PtrToStructure<T>(buffer);
            Marshal.FreeHGlobal(buffer);
            return obj;
        }
        public void Write<T>(int offset, T data) where T : class, new() {
            var size = Marshal.SizeOf<T>();
            if (size + offset > Size) {
                throw new ArgumentOutOfRangeException();
            }

            var buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, buffer, true);
            Kernel.WriteProcessMemory(m_processHandle, offset, buffer, size, out var _);
            Marshal.FreeHGlobal(buffer);
        }

        protected override bool ReleaseHandle() {
            return Kernel.VirtualFreeEx(m_processHandle, handle);
        }
    }
}
