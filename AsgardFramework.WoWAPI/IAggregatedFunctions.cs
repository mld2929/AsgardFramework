using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Info;
using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    internal interface IAggregatedFunctions
    {
        Task<IEnumerable<LootSlotInfo>> GetLootSlotsInfo(Range range);
        Task<IEnumerable<SpellInfo>> GetSpellsInfo(IEnumerable<int> ids);
        Task<IEnumerable<SpellCooldownInfo>> GetSpellsCooldown(IEnumerable<int> ids);
        Task<IEnumerable<UnitAuraInfo>> GetUnitAurasInfo(string unitName, Range range, string filter);
        Task<IEnumerable<Position>> GetPositions(IEnumerable<int> objBases);
        Task<IEnumerable<string>> GetNames(IEnumerable<int> objBases);
    }
}
