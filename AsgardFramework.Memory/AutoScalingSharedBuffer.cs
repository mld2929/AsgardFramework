using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    // work in progress, now buffer is very slow and fragmented
    internal sealed class AutoScalingSharedBuffer : AutoManagedSharedBuffer
    {
        #region Constructors

        internal AutoScalingSharedBuffer(IntPtr address, SafeHandle processHandle, int size, Func<int, IAutoManagedSharedBuffer> fabric) : base(address, processHandle, size) {
            m_fabric = fabric;
        }

        #endregion Constructors

        #region Properties

        public override int Size => base.Size + m_additionalSize;

        #endregion Properties

        #region Methods

        public override bool TryReserve(int size, out IAutoManagedMemory reserved) {
            reserved = null;

            lock (m_lock) {
                if (size <= base.Size && base.TryReserve(size, out reserved))
                    return reserved != null;

                foreach (var buffer in m_additional.Where(buffer => size <= buffer.Size))
                    if (buffer.TryReserve(size, out reserved))
                        break;

                if (reserved != null)
                    return reserved != null;

                var newBuffer = m_fabric(size);
                m_additional.Insert(0, newBuffer);
                m_additionalSize += newBuffer.Size;
                newBuffer.TryReserve(size, out reserved);
            }

            return reserved != null;
        }

        #endregion Methods

        #region Fields

        private readonly List<IAutoManagedSharedBuffer> m_additional = new List<IAutoManagedSharedBuffer>();
        private readonly Func<int, IAutoManagedSharedBuffer> m_fabric;
        private readonly object m_lock = new object();
        private int m_additionalSize;

        #endregion Fields
    }
}