using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    internal class AutoManagedMemory : KernelDrivenMemoryBase, IAutoManagedMemory
    {
        #region Constructors

        internal AutoManagedMemory(IntPtr address, SafeHandle processHandle, int size) : base(processHandle) {
            handle = address;

            if (IsInvalid)
                throw new ArgumentException("Invalid address", nameof(address));

            Size = size;
        }

        #endregion Constructors

        #region Properties

        public sealed override bool IsInvalid => handle == IntPtr.Zero;

        public virtual int Size { get; }

        public int Start => handle.ToInt32();

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Converts virtual offset to absolute
        /// </summary>
        public sealed override byte[] Read(int offset, int count) {
            return base.Read(Start + offset, count);
        }

        /// <summary>
        ///     Converts virtual offset to absolute
        /// </summary>
        public sealed override void Write(int offset, byte[] data) {
            base.Write(Start + offset, data);
        }

        protected override bool ReleaseHandle() {
            return Kernel.VirtualFreeEx(m_processHandle, handle);
        }

        #endregion Methods
    }
}