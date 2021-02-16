using System.Collections.Generic;

namespace AsgardFramework.WoWAPI
{
    public interface ISpellBook
    {
        IEnumerable<int> GetSpells();
    }
}
