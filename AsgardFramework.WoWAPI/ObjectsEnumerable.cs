using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;
using System.Collections;
using System.Collections.Generic;

namespace AsgardFramework.WoWAPI
{
    internal class ObjectsEnumerator : IEnumerator<ObjectData>
    {
        private readonly int m_first;
        private readonly IGlobalMemory m_memory;
        private int m_current;
        internal ObjectsEnumerator(IGlobalMemory memory, int first) {
            m_first = first;
            m_memory = memory;
        }
        public ObjectData Current => new ObjectData(m_current, m_memory.Read<Common>(m_current), null, null);

        object IEnumerator.Current => Current;

        public void Dispose() { }
        public bool MoveNext() {
            m_current = m_current == 0 ? m_first : Current.Common.Next;
            return !(m_current == 0 || m_current % 2 != 0);
        }
        public void Reset() {
            m_current = m_first;
        }
    }

    internal class ObjectsEnumerable : IEnumerable<ObjectData>
    {
        private readonly ObjectsEnumerator m_enumerator;
        internal ObjectsEnumerable(IGlobalMemory memory, int first) {
            m_enumerator = new ObjectsEnumerator(memory, first);
        }
        public IEnumerator<ObjectData> GetEnumerator() {
            return m_enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return m_enumerator;
        }
    }
}
