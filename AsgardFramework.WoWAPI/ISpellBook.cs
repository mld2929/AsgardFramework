using System.Collections.Generic;

namespace AsgardFramework.WoWAPI
{
    public interface ISpellBook
    {
        #region Methods

        IEnumerable<int> GetSpells();

        #endregion Methods
    }
}