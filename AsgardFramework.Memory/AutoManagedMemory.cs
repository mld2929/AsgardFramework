using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Indexers

        public IEnumerable<byte> this[Range range] {
            get {
                var (offset, count) = convertRange(range);

                return Read(offset, count);
            }
            set {
                var (offset, count) = convertRange(range);

                if (count != value.Count())
                    throw new ArgumentOutOfRangeException();

                Write(offset, value.ToArray());
            }
        }

        public byte this[Index index] {
            get => Read(index.IsFromEnd ? Size - index.Value : index.Value);
            set => Write(index.IsFromEnd ? Size - index.Value : index.Value, value);
        }

        #endregion Indexers

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

        private (int, int) convertRange(Range range) {
            if (range.Equals(Range.All))
                return (0, Size);

            var start = range.Start.IsFromEnd ? Size - range.Start.Value : range.Start.Value;
            var end = range.End.IsFromEnd ? Size - range.End.Value : range.End.Value;

            return (Math.Min(start, end), Math.Abs(start - end));
        }

        #endregion Methods
    }
}