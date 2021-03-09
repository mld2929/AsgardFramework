using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    internal class AutoManagedSharedBuffer : AutoManagedMemory, IAutoManagedSharedBuffer
    {
        #region Constructors

        internal AutoManagedSharedBuffer(IntPtr address, SafeHandle processHandle, int size) : base(address, processHandle, size) {
            m_blocks = new List<SharedBlockData> {
                new SharedBlockData(address.ToInt32(), size)
            };
        }

        #endregion Constructors

        #region Fields

        private readonly List<SharedBlockData> m_blocks;

        private readonly object m_lock = new object();

        #endregion Fields

        #region Methods

        public IAutoManagedMemory Reserve(int size) {
            if (!TryReserve(size, out var reserved))
                throw new InvalidOperationException("Can't reserve memory");

            return reserved;
        }

        public bool TryReserve(int size, out IAutoManagedMemory reserved) {
            reserved = reserveBlock(size);

            return reserved != null;
        }

        protected internal virtual SharedBlock reserveBlock(int size) {
            lock (m_lock) {
                var min = m_blocks.Where(b => !b.Reserved && b.Size >= size)
                                  .Min();

                if (min == null) {
                    mergeBlocks();

                    min = m_blocks.Where(b => !b.Reserved && b.Size >= size)
                                  .Min();
                }

                if (min == null)
                    return null;

                if (min.Size - size != 0) {
                    var (left, right) = min.Split(size);
                    var index = m_blocks.IndexOf(min);
                    m_blocks.RemoveAt(index);
                    m_blocks.Insert(index, right);
                    m_blocks.Insert(index, left);
                    min = left;
                }

                return new SharedBlock(min, m_processHandle);
            }
        }

        private void mergeBlocks() {
            var count = m_blocks.Count - 1;

            for (var i = 0; i < count; i++) {
                var merged = m_blocks[i]
                    .Concat(m_blocks[i + 1]);

                if (merged == null)
                    continue;

                m_blocks.Insert(i, merged);
                m_blocks.RemoveRange(i + 1, 2);
                i--;
                count--;
            }
        }

        #endregion Methods
    }

    internal class SharedBlock : AutoManagedMemory
    {
        #region Fields

        protected internal readonly SharedBlockData m_data;

        #endregion Fields

        #region Constructors

        internal SharedBlock(SharedBlockData data, SafeHandle processHandle) : base((IntPtr)data.StartAddress, processHandle, data.Size) {
            m_data = data;
            m_data.Reserved = true;
        }

        #endregion Constructors

        #region Methods

        protected override bool ReleaseHandle() {
            m_data.Reserved = false;

            return true;
        }

        #endregion Methods
    }

    internal class SharedBlockData : IComparable<SharedBlockData>
    {
        #region Constructors

        internal SharedBlockData(int start, int size) {
            StartAddress = start;
            Size = size;
        }

        #endregion Constructors

        #region Fields

        internal readonly int Size;

        internal readonly int StartAddress;

        internal bool Reserved;

        #endregion Fields

        #region Methods

        public int CompareTo([AllowNull] SharedBlockData other) {
            return other switch {
                null => 1,
                _ => Size.CompareTo(other.Size)
            };
        }

        internal SharedBlockData Concat(SharedBlockData r) {
            if (Reserved || r.Reserved)
                return null;

            if (StartAddress + Size != r.StartAddress && r.StartAddress + r.Size != StartAddress)
                return null;

            return new SharedBlockData(Math.Min(StartAddress, r.StartAddress), Size + r.Size);
        }

        internal (SharedBlockData, SharedBlockData) Split(int leftSize) {
            return (new SharedBlockData(StartAddress, leftSize), new SharedBlockData(StartAddress + leftSize, Size - leftSize));
        }

        #endregion Methods
    }
}