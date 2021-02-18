using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    internal class AutoManagedSharedBuffer : AutoManagedMemory, IAutoManagedSharedBuffer
    {
        private readonly List<SharedBlockData> m_blocks;
        private readonly object m_lock = new object();
        internal AutoManagedSharedBuffer(IntPtr address, SafeHandle processHandle, int size) : base(address, processHandle, size) {
            m_blocks = new List<SharedBlockData>() { new SharedBlockData(address.ToInt32(), size) };
        }

        public virtual bool TryReserve(int size, out IAutoManagedMemory reserved) {
            reserved = null;
            lock (m_lock) {
                var min = m_blocks.Where(b => !b.Reserved && b.Size >= size).Min();
                if (min == null) {
                    MergeBlocks();
                    min = m_blocks.Where(b => !b.Reserved && b.Size >= size).Min();
                }
                if (min != null) {
                    if (min.Size - size != 0) {
                        (var left, var right) = min.Split(size);
                        var index = m_blocks.IndexOf(min);
                        m_blocks.RemoveAt(index);
                        m_blocks.Insert(index, right);
                        m_blocks.Insert(index, left);
                        min = left;
                    }
                    reserved = new SharedBlock(min, m_processHandle);
                }
            }
            return reserved != null;
        }

        private void MergeBlocks() {
            for (var i = 0; i < m_blocks.Count() - 1; i++) {
                if (m_blocks[i].Concat(m_blocks[i + 1]) is SharedBlockData merged) {
                    m_blocks.Insert(i, merged);
                    m_blocks.RemoveRange(i + 1, 2);
                    i--;
                }
            }
        }
    }

    internal class SharedBlock : AutoManagedMemory
    {
        private readonly SharedBlockData m_data;
        internal SharedBlock(SharedBlockData data, SafeHandle processHandle) : base((IntPtr)data.StartAddress, processHandle, data.Size) {
            m_data = data;
            m_data.Reserved = true;
        }
        protected override bool ReleaseHandle() {
            m_data.Reserved = false;
            return true;
        }
    }

    internal class SharedBlockData : IComparable<SharedBlockData>
    {
        internal readonly int StartAddress;
        internal readonly int Size;
        internal bool Reserved;
        internal SharedBlockData(int start, int size) {
            StartAddress = start;
            Size = size;
        }

        public int CompareTo([AllowNull] SharedBlockData other) {
            if (other == null) {
                return 1;
            }

            return Size.CompareTo(other.Size);
        }

        internal SharedBlockData Concat(SharedBlockData r) {
            if (Reserved || r.Reserved) {
                return null;
            }

            if (StartAddress + Size != r.StartAddress && r.StartAddress + r.Size != StartAddress) {
                return null;
            }

            return new SharedBlockData(Math.Min(StartAddress, r.StartAddress), Size + r.Size);
        }

        internal (SharedBlockData, SharedBlockData) Split(int leftSize) {
            return (new SharedBlockData(StartAddress, leftSize), new SharedBlockData(StartAddress + leftSize, Size - leftSize));
        }
    }
}
