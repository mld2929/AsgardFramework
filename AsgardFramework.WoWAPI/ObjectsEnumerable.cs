using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AsgardFramework.WoWAPI
{
    class ObjectsEnumerator : IEnumerator<Common>
    {
        private readonly int m_first;
        private readonly IGlobalMemory m_memory;
        private int m_current;
        internal ObjectsEnumerator(IGlobalMemory memory, int first)
        {
            m_first = first;
            m_memory = memory;
        }
        public Common Current => m_memory.Read<Common>(m_current);

        object IEnumerator.Current => Current;

        public void Dispose() { }
        public bool MoveNext()
        {
            m_current = Current.Next;
            return !(m_current == 0 || m_current % 2 != 0);
        }
        public void Reset()
        {
            m_current = m_first;
        }
    }

    class ObjectsEnumerable : IEnumerable<Common>
    {
        private readonly ObjectsEnumerator m_enumerator;
        internal ObjectsEnumerable(IGlobalMemory memory, int first)
        {
            m_enumerator = new ObjectsEnumerator(memory, first);
        }
        public IEnumerator<Common> GetEnumerator() => m_enumerator;
        IEnumerator IEnumerable.GetEnumerator() => m_enumerator;
    }
}
