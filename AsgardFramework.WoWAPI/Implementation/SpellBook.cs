using System.Collections.Generic;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class SpellBook : ISpellBook
    {
        #region Constructors

        internal SpellBook(IGlobalMemory memory) {
            m_memory = memory;
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<int> GetSpells() {
            return m_memory.Read<int>(c_spellBook, m_memory.Read<int>(c_spellCount));
        }

        #endregion Methods

        #region Fields

        private const int c_spellBook = 0x00BE5D88;

        private const int c_spellCount = 0x00BE8D9C;

        private readonly IGlobalMemory m_memory;

        #endregion Fields
    }
}