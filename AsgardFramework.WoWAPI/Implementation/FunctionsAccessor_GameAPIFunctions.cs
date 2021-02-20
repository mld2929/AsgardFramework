using System;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Info;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IGameAPIFunctions
    {
        #region Methods

        public Task AttackTargetAsync() {
            throw new NotImplementedException();
        }

        public Task DismountAsync() {
            throw new NotImplementedException();
        }

        public Task EquipItemAsync(string name) {
            throw new NotImplementedException();
        }

        public Task EquipItemAsync(int itemId) {
            throw new NotImplementedException();
        }

        public Task<ItemSpellInfo> GetItemSpellAsync(string itemName) {
            throw new NotImplementedException();
        }

        public Task<ItemSpellInfo> GetItemSpellAsync(int itemId) {
            throw new NotImplementedException();
        }

        public Task<LootSlotInfo> GetLootSlotInfoAsync(int slotNumberFromOne) {
            throw new NotImplementedException();
        }

        public Task<int> GetNumLootItemsAsync() {
            throw new NotImplementedException();
        }

        public Task<SpellCooldownInfo> GetSpellCooldownAsync(string name) {
            throw new NotImplementedException();
        }

        public Task<SpellCooldownInfo> GetSpellCooldownAsync(int spellId) {
            throw new NotImplementedException();
        }

        public Task<SpellInfo> GetSpellInfoAsync(int spellId) {
            throw new NotImplementedException();
        }

        public Task<SpellInfo> GetSpellInfoAsync(string spellName) {
            throw new NotImplementedException();
        }

        public Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, int index, string filter = null) {
            throw new NotImplementedException();
        }

        public Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, string auraName, string auraSecondaryText = null, string filter = null) {
            throw new NotImplementedException();
        }

        public Task<UnitClassInfo> GetUnitClassAsync(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        public Task<bool> HasPlayerFullControlAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> IsPlayerFallingAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> IsLoggedInAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> IsMountedAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> IsOutdoorsAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> UnitIsPlayerAsync(string unitMetaId) {
            throw new NotImplementedException();
        }

        public Task<bool> IsUnitAffectingCombatAsync(string unitMetaId) {
            throw new NotImplementedException();
        }

        public Task LootSlotAsync(int slotNumberFromOne) {
            throw new NotImplementedException();
        }

        public Task RunScriptAsync(string luaScript) {
            throw new NotImplementedException();
        }

        public Task StartFollowUnitAsync(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        public Task<UnitFactionGroupInfo> GetUnitFactionGroupAsync(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}