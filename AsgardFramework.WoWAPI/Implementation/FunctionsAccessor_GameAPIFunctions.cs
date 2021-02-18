using System;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Info;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IGameAPIFunctions
    {
        public Task AttackTarget() {
            throw new NotImplementedException();
        }

        public Task Dismount() {
            throw new NotImplementedException();
        }

        public Task EquipItem(string name) {
            throw new NotImplementedException();
        }

        public Task EquipItem(int itemId) {
            throw new NotImplementedException();
        }

        public Task<ItemSpellInfo> GetItemSpell(string itemName) {
            throw new NotImplementedException();
        }

        public Task<ItemSpellInfo> GetItemSpell(int itemId) {
            throw new NotImplementedException();
        }

        public Task<LootSlotInfo> GetLootSlotInfo(int slotNumberFromOne) {
            throw new NotImplementedException();
        }

        public Task<int> GetNumLootItems() {
            throw new NotImplementedException();
        }

        public Task<SpellCooldownInfo> GetSpellCooldown(string name) {
            throw new NotImplementedException();
        }

        public Task<SpellCooldownInfo> GetSpellCooldown(int spellId) {
            throw new NotImplementedException();
        }

        public Task<SpellInfo> GetSpellInfo(int spellId) {
            throw new NotImplementedException();
        }

        public Task<SpellInfo> GetSpellInfo(string spellName) {
            throw new NotImplementedException();
        }

        public Task<UnitAuraInfo> GetUnitAura(string unitMetaId, int index, string filter = null) {
            throw new NotImplementedException();
        }

        public Task<UnitAuraInfo> GetUnitAura(string unitMetaId, string auraName, string auraSecondaryText = null, string filter = null) {
            throw new NotImplementedException();
        }

        public Task<UnitClassInfo> GetUnitClass(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        public Task<bool> HasFullControl() {
            throw new NotImplementedException();
        }

        public Task<bool> IsFalling() {
            throw new NotImplementedException();
        }

        public Task<bool> IsLoggedIn() {
            throw new NotImplementedException();
        }

        public Task<bool> IsMounted() {
            throw new NotImplementedException();
        }

        public Task<bool> IsOutdoors() {
            throw new NotImplementedException();
        }

        public Task<bool> IsPlayer(string unitMetaId) {
            throw new NotImplementedException();
        }

        public Task<bool> IsUnitAffectingCombat(string unitMetaId) {
            throw new NotImplementedException();
        }

        public Task LootSlot(int slotNumberFromOne) {
            throw new NotImplementedException();
        }

        public Task RunScript(string luaScript) {
            throw new NotImplementedException();
        }

        public Task StartFollowUnit(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        public Task<UnitFactionGroupInfo> UnitFactionGroup(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }
    }
}
