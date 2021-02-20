using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Info;
using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    public interface IAggregatedFunctions
    {
        #region Methods

        Task<IEnumerable<LootSlotInfo>> GetLootSlotsInfoAsync(Range range);

        Task<IEnumerable<string>> GetNamesAsync(IEnumerable<int> objBases);

        Task<IEnumerable<Position>> GetPositionsAsync(IEnumerable<int> objBases);

        Task<IEnumerable<SpellCooldownInfo>> GetSpellsCooldownAsync(IEnumerable<int> ids);

        Task<IEnumerable<SpellInfo>> GetSpellsInfoAsync(IEnumerable<int> ids);

        Task<IEnumerable<UnitAuraInfo>> GetUnitAurasInfoAsync(string unitName, Range range, string filter);

        #endregion Methods
    }
}