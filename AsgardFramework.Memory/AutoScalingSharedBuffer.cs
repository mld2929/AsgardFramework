using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{

    // work in progress, now buffer is very slow and fragmented
    internal class AutoScalingSharedBuffer : AutoManagedSharedBuffer
    {
        private readonly List<IAutoManagedSharedBuffer> m_additional = new List<IAutoManagedSharedBuffer>();
        private readonly Func<int, IAutoManagedSharedBuffer> m_fabric;
        private int m_additionalSize = 0;
        private readonly object m_lock = new object();
        public override int Size => base.Size + m_additionalSize;
        internal AutoScalingSharedBuffer(IntPtr address, SafeHandle processHandle, int size, Func<int, IAutoManagedSharedBuffer> fabric) : base(address, processHandle, size) {
            m_fabric = fabric;
        }

        public override bool TryReserve(int size, out IAutoManagedMemory reserved) {
            reserved = null;
            lock (m_lock) {
                if (size > base.Size || !base.TryReserve(size, out reserved)) {
                    foreach (var buffer in m_additional) {
                        if (size > buffer.Size) {
                            continue;
                        }

                        if (buffer.TryReserve(size, out reserved)) {
                            break;
                        }
                    }
                    if (reserved == null) {
                        var newBuffer = m_fabric(size);
                        m_additional.Insert(0, newBuffer);
                        m_additionalSize += newBuffer.Size;
                        newBuffer.TryReserve(size, out reserved);
                    }
                }
            }
            return reserved != null;
        }
    }
}
