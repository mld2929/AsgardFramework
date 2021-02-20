using System;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

namespace AsgardFramework.Memory
{
    internal class AutoManagedMemory : SafeHandleZeroOrMinusOneIsInvalid, IAutoManagedMemory
    {
        #region Fields

        protected readonly SafeHandle m_processHandle;

        #endregion Fields

        #region Constructors

        internal AutoManagedMemory(IntPtr address, SafeHandle processHandle, int size) : base(true) {
            handle = address;

            if (IsInvalid)
                throw new InvalidOperationException();

            var weakThis = new WeakReference<AutoManagedMemory>(this);

            AppDomain.CurrentDomain.ProcessExit += (_, __) => {
                if (weakThis.TryGetTarget(out var @this))
                    @this.Dispose();
            };

            m_processHandle = processHandle;
            Size = size;
        }

        public sealed override bool IsInvalid => base.IsInvalid;

        #endregion Constructors

        #region Properties

        public virtual int Size { get; }

        public int Start => handle.ToInt32();

        #endregion Properties

        #region Methods

        public byte[] Read(int offset, int count) {
            if (count + offset > Size)
                throw new ArgumentOutOfRangeException(nameof(count), $"{count + offset - Size} bytes out of range");

            var result = new byte[count];
            Kernel.ReadProcessMemory(m_processHandle, handle + offset, result, count, out var _);

            return result;
        }

        public T Read<T>(int offset) where T : new() {
            var size = Marshal.SizeOf<T>();

            if (size + offset > Size) {
                var typeName = typeof(T).Name;
                throw new ArgumentOutOfRangeException(typeName, $"{size + offset - Size} bytes of {typeName} out of range");
            }
               

            var bytes = new byte[size];
            Kernel.ReadProcessMemory(m_processHandle, offset, bytes, bytes.Length, out var _);

            unsafe {
                fixed (byte* buffer = bytes) {
                    return Marshal.PtrToStructure<T>((IntPtr)buffer);
                }
            }
        }

        public void Write(int offset, byte[] data) {
            if (data.Length + offset > Size)
                throw new ArgumentOutOfRangeException(nameof(data), $"{data.Length + offset - Size} bytes out of range");

            Kernel.WriteProcessMemory(m_processHandle, handle + offset, data, data.Length, out var _);
        }

        public void Write<T>(int offset, T data) where T : new() {
            var size = Marshal.SizeOf<T>();

            if (size + offset > Size)
                throw new ArgumentOutOfRangeException(nameof(data), $"{size + offset - Size} bytes of {nameof(data)} out of range");

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

        #endregion Methods
    }
}