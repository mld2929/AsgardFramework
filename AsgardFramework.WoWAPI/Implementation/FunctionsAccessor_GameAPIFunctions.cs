using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.LuaData;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal static class TaskHelper
    {
        #region Methods

        public static async Task<TTo> Cast<TTo, TSource>(this Task<TSource> task) where TSource : LuaValue<TTo> {
            return await task.ConfigureAwait(false);
        }

        #endregion Methods
    }

    internal partial class FunctionsAccessor : IGameAPIFunctions
    {
        #region Methods

        public Task AttackTargetAsync() {
            return m_luaExecutor.RunScriptAsync("AttackTarget()");
        }

        public Task DismountAsync() {
            return m_luaExecutor.RunScriptAsync("Dismount()");
        }

        public Task EquipItemAsync(string name) {
            return m_luaExecutor.RunScriptAsync($"EquipItemByName({name})");
        }

        public Task EquipItemAsync(int itemId) {
            return m_luaExecutor.RunScriptAsync($"EquipItemByName({itemId})");
        }

        public async Task<(int freeSlots, BagType bagType)> GetContainerNumFreeSlotsAsync(int containerId) {
            var data = (List<string>)await m_luaExecutor.RunScriptAsync<LuaStringList>($"GetContainerNumFreeSlots({containerId})", 2)
                                                        .ConfigureAwait(false);

            return (data[0]
                        .ToInt(), data[1]
                        .ToEnum<BagType>());
        }

        public Task<ItemSpellInfo> GetItemSpellAsync(string itemName) {
            return m_luaExecutor.RunScriptAsync<ItemSpellInfo>($"GetItemSpell({itemName})");
        }

        public Task<ItemSpellInfo> GetItemSpellAsync(int itemId) {
            return m_luaExecutor.RunScriptAsync<ItemSpellInfo>($"GetItemSpell({itemId})");
        }

        public Task<LootSlotInfo> GetLootSlotInfoAsync(int slotNumberFromOne) {
            return m_luaExecutor.RunScriptAsync<LootSlotInfo>($"GetLootSlotInfo({slotNumberFromOne})");
        }

        public async Task<int> GetNumLootItemsAsync() {
            return (int)await m_luaExecutor.RunScriptAsync<LuaNumber>("GetNumLootItems()", 1)
                                           .ConfigureAwait(false);
        }

        public Task<SpellCooldownInfo> GetSpellCooldownAsync(string name) {
            return m_luaExecutor.RunScriptAsync<SpellCooldownInfo>($"GetSpellCooldown({name})");
        }

        public Task<SpellCooldownInfo> GetSpellCooldownAsync(int spellId) {
            return m_luaExecutor.RunScriptAsync<SpellCooldownInfo>($"GetSpellCooldown({spellId})");
        }

        public Task<SpellInfo> GetSpellInfoAsync(int spellId) {
            return m_luaExecutor.RunScriptAsync<SpellInfo>($"GetSpellInfo({spellId})");
        }

        public Task<SpellInfo> GetSpellInfoAsync(string spellName) {
            return m_luaExecutor.RunScriptAsync<SpellInfo>($"GetSpellInfo({spellName})");
        }

        public Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, int index, string filter = null) {
            var strFilter = filter != null ? $", {filter}" : string.Empty;

            return m_luaExecutor.RunScriptAsync<UnitAuraInfo>($"UnitAura({unitMetaId}, {index}{strFilter})", 11);
        }

        public Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, string auraName, string auraSecondaryText = null, string filter = null) {
            var strSecondary = auraSecondaryText != null ? $", {auraSecondaryText}" : string.Empty;
            var strFilter = filter != null ? $", {filter}" : string.Empty;

            return m_luaExecutor.RunScriptAsync<UnitAuraInfo>($"UnitAura({unitMetaId}, {auraName}{strSecondary}{strFilter})", 11);
        }

        public Task<UnitClassInfo> GetUnitClassAsync(string unitMetaIdOrName) {
            return m_luaExecutor.RunScriptAsync<UnitClassInfo>($"UnitClass({unitMetaIdOrName})", 2);
        }

        public Task<UnitFactionGroupInfo> GetUnitFactionGroupAsync(string unitMetaIdOrName) {
            return m_luaExecutor.RunScriptAsync<UnitFactionGroupInfo>($"UnitFactionGroup({unitMetaIdOrName})", 2);
        }

        public Task<bool> HasPlayerFullControlAsync() {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>("HasFullControll()", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsLoggedInAsync() {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>("IsLoggedIn()", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsMountedAsync() {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>("IsMounted()", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsOutdoorsAsync() {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>("IsOutdoors()", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsPlayerFallingAsync() {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>("IsFalling()", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsUnitAffectingCombatAsync(string unitMetaId) {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>($"UnitAffectingCombat({unitMetaId})", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task LootSlotAsync(int slotNumberFromOne) {
            return m_luaExecutor.RunScriptAsync($"LootSlot({slotNumberFromOne})");
        }

        public Task StartFollowUnitAsync(string unitMetaIdOrName) {
            return m_luaExecutor.RunScriptAsync($"FollowUnit({unitMetaIdOrName})");
        }

        public Task<bool> UnitIsConnectedAsync(string unitMetaId) {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>($"UnitIsConnected({unitMetaId})", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> UnitIsDeadOrGhostAsync(string unitMetaId) {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>($"UnitIsDeadOrGhost({unitMetaId})", 1)
                                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> UnitIsPlayerAsync(string unitMetaId) {
            return m_luaExecutor.RunScriptAsync<LuaBoolean>($"UnitIsPlayer({unitMetaId})", 1)
                                .Cast<bool, LuaBoolean>();
        }

        private IAutoManagedMemory stringToPtr(string value) {
            var size = Encoding.UTF8.GetByteCount(value) + 1;

            var reserved = m_buffer.Reserve(size);
            reserved.WriteNullTerminatedString(0, value, Encoding.UTF8);

            return reserved;
        }

        #endregion Methods
    }
}