using System;
using System.Collections;
using System.Collections.Generic;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI.Utils
{
    internal class ObjectsEnumerable : IEnumerable<ObjectData>
    {
        #region Fields

        private readonly ObjectsEnumerator m_enumerator;

        #endregion Fields

        #region Constructors

        internal ObjectsEnumerable(IGlobalMemory memory, int first) {
            m_enumerator = new ObjectsEnumerator(memory, first);
        }

        #endregion Constructors

        #region Methods

        public IEnumerator<ObjectData> GetEnumerator() {
            return m_enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return m_enumerator;
        }

        #endregion Methods
    }

    internal class ObjectsEnumerator : IEnumerator<ObjectData>
    {
        #region Constructors

        internal ObjectsEnumerator(IGlobalMemory memory, int first) {
            m_first = first;
            m_memory = memory;
        }

        #endregion Constructors

        #region Fields

        private readonly int m_first;
        private readonly IGlobalMemory m_memory;
        private int m_current;

        #endregion Fields

        #region Properties

        public ObjectData Current => new ObjectData(m_current, m_memory.Read<Common>(m_current), null, null);

        object IEnumerator.Current => Current;

        #endregion Properties

        #region Methods

        public void Dispose() {
            // nothing to release
        }

        public bool MoveNext() {
            m_current = m_current == 0 ? m_first : Current?.Common.Next ?? throw new InvalidOperationException($"{nameof(Current)} is null");

            return !(m_current == 0 || m_current % 2 != 0);
        }

        public void Reset() {
            m_current = m_first;
        }

        #endregion Methods
    }
}