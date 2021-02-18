using System.Collections.Generic;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class SpellBook : ISpellBook
    {
        private const int c_spellCount = 0x00BE8D9C;
        private const int c_spellBook = 0x00BE5D88;
        private readonly IGlobalMemory m_memory;
        internal SpellBook(IGlobalMemory memory) {
            m_memory = memory;
        }
        public IEnumerable<int> GetSpells() {
            var count = m_memory.Read(c_spellCount, 4).ToInt32();
            return m_memory.Read(c_spellBook, count * 4).ToArrayOfInt32();
        }
    }
}
