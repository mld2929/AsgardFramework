using System;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace AsgardFramework.Memory
{
    internal class AutoManagedMemory : SafeHandleZeroOrMinusOneIsInvalid, IAutoManagedMemory
    {
        protected readonly SafeHandle m_processHandle;
        internal AutoManagedMemory(IntPtr address, SafeHandle processHandle, int size) : base(true) {
            handle = address;
            if (IsInvalid) {
                throw new InvalidOperationException();
            }

            m_processHandle = processHandle;
            Size = size;
        }

        public virtual int Size { get; private set; }

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

        public T Read<T>(int offset) where T : new() {
            var size = Marshal.SizeOf<T>();
            if (size + offset > Size) {
                throw new ArgumentOutOfRangeException();
            }
            var bytes = new byte[size];
            Kernel.ReadProcessMemory(m_processHandle, offset, bytes, bytes.Length, out var _);
            unsafe {
                fixed (byte* buffer = bytes) {
                    return Marshal.PtrToStructure<T>((IntPtr)buffer);
                }
            }
        }
        public void Write<T>(int offset, T data) where T : new() {
            var size = Marshal.SizeOf<T>();
            if (size + offset > Size) {
                throw new ArgumentOutOfRangeException();
            }
            var bytes = new byte[size];
            unsafe {
                fixed (byte* buffer = bytes) {
                    Marshal.StructureToPtr(data, (IntPtr)buffer, true);
                }
            }
            Kernel.WriteProcessMemory(m_processHandle, offset, bytes, bytes.Length, out var _);
        }

        protected override bool ReleaseHandle() {
            return Kernel.VirtualFreeEx(m_processHandle, handle);
        }
    }
}
