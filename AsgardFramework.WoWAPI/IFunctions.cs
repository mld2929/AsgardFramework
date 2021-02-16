using AsgardFramework.WoWAPI.Info;
using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IFunctions
    {
        Task<int> GetNumLootItems();
        Task LootSlot(int slotNumberFromOne);
        Task<LootSlotInfo> GetLootSlotInfo(int slotNumberFromOne);
        Task CastSpell(int spellId, ulong target);
        Task Interact(int objBase);
        Task<string> GetName(int objBase);
        Task UpdatePosition(Objects.ObjectData obj);
        Task<SpellInfo> GetSpellInfo(int spellId);
        Task<SpellInfo> GetSpellInfo(string spellName);
        Task SellItem(ulong itemGuid, ulong vendorGuid);
    }
}
