using System.Collections.Generic;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Utils;

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
            var count = m_memory.Read<int>(c_spellCount);

            return m_memory.Read(c_spellBook, count * 4)
                           .ToArrayOfInt32();
        }

        #endregion Methods

        #region Fields

        private const int c_spellBook = 0x00BE5D88;
        private const int c_spellCount = 0x00BE8D9C;
        private readonly IGlobalMemory m_memory;

        #endregion Fields
    }
}