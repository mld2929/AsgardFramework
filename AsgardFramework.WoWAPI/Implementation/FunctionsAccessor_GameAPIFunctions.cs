using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.LuaData;
using AsgardFramework.WoWAPI.Utils;

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
            return RunScriptAsync("AttackTarget()");
        }

        public Task DismountAsync() {
            return RunScriptAsync("Dismount()");
        }

        public Task EquipItemAsync(string name) {
            return RunScriptAsync($"EquipItemByName({name})");
        }

        public Task EquipItemAsync(int itemId) {
            return RunScriptAsync($"EquipItemByName({itemId})");
        }

        public async Task<(int freeSlots, BagType bagType)> GetContainerNumFreeSlotsAsync(int containerId) {
            var data = (List<string>)await RunScriptAsync<LuaStringList>($"GetContainerNumFreeSlots({containerId})", 2)
                                         .ConfigureAwait(false);

            return (data[0]
                        .ToInt(), data[1]
                        .ToEnum<BagType>());
        }

        public Task<ItemSpellInfo> GetItemSpellAsync(string itemName) {
            return RunScriptAsync<ItemSpellInfo>($"GetItemSpell({itemName})");
        }

        public Task<ItemSpellInfo> GetItemSpellAsync(int itemId) {
            return RunScriptAsync<ItemSpellInfo>($"GetItemSpell({itemId})");
        }

        public Task<LootSlotInfo> GetLootSlotInfoAsync(int slotNumberFromOne) {
            return RunScriptAsync<LootSlotInfo>($"GetLootSlotInfo({slotNumberFromOne})");
        }

        public async Task<int> GetNumLootItemsAsync() {
            return (int)await RunScriptAsync<LuaNumber>("GetNumLootItems()", 1)
                            .ConfigureAwait(false);
        }

        public Task<SpellCooldownInfo> GetSpellCooldownAsync(string name) {
            return RunScriptAsync<SpellCooldownInfo>($"GetSpellCooldown({name})");
        }

        public Task<SpellCooldownInfo> GetSpellCooldownAsync(int spellId) {
            return RunScriptAsync<SpellCooldownInfo>($"GetSpellCooldown({spellId})");
        }

        public Task<SpellInfo> GetSpellInfoAsync(int spellId) {
            return RunScriptAsync<SpellInfo>($"GetSpellInfo({spellId})");
        }

        public Task<SpellInfo> GetSpellInfoAsync(string spellName) {
            return RunScriptAsync<SpellInfo>($"GetSpellInfo({spellName})");
        }

        public Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, int index, string filter = null) {
            var strFilter = filter != null ? $", {filter}" : string.Empty;

            return RunScriptAsync<UnitAuraInfo>($"UnitAura({unitMetaId}, {index}{strFilter})", 11);
        }

        public Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, string auraName, string auraSecondaryText = null, string filter = null) {
            var strSecondary = auraSecondaryText != null ? $", {auraSecondaryText}" : string.Empty;
            var strFilter = filter != null ? $", {filter}" : string.Empty;

            return RunScriptAsync<UnitAuraInfo>($"UnitAura({unitMetaId}, {auraName}{strSecondary}{strFilter})", 11);
        }

        public Task<UnitClassInfo> GetUnitClassAsync(string unitMetaIdOrName) {
            return RunScriptAsync<UnitClassInfo>($"UnitClass({unitMetaIdOrName})", 2);
        }

        public Task<UnitFactionGroupInfo> GetUnitFactionGroupAsync(string unitMetaIdOrName) {
            return RunScriptAsync<UnitFactionGroupInfo>($"UnitFactionGroup({unitMetaIdOrName})", 2);
        }

        public Task<bool> HasPlayerFullControlAsync() {
            return RunScriptAsync<LuaBoolean>("HasFullControll()", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsLoggedInAsync() {
            return RunScriptAsync<LuaBoolean>("IsLoggedIn()", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsMountedAsync() {
            return RunScriptAsync<LuaBoolean>("IsMounted()", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsOutdoorsAsync() {
            return RunScriptAsync<LuaBoolean>("IsOutdoors()", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsPlayerFallingAsync() {
            return RunScriptAsync<LuaBoolean>("IsFalling()", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> IsUnitAffectingCombatAsync(string unitMetaId) {
            return RunScriptAsync<LuaBoolean>($"UnitAffectingCombat({unitMetaId})", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task LootSlotAsync(int slotNumberFromOne) {
            return RunScriptAsync($"LootSlot({slotNumberFromOne})");
        }

        // todo: rewrite
        public async Task RunScriptAsync(string luaScript) {
            const int runScript = 0x004DD490;
            var pScript = stringToPtr(luaScript);

            //await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pScript)
            //                                                .CallLuaFunction(runScript)
            //                                                .CompileScript(m_assembler))
            //.ConfigureAwait(false);

            pScript.Dispose();
        }

        // todo: rewrite
        public async Task<T> RunScriptAsync<T>(string luaScript, int fieldsCount = 10) where T : LuaValue, new() {
            const int runScript = 0x004DD490;
            var sb = new StringBuilder();

            for (var i = 0; i < fieldsCount; i++) {
                sb.Append('v');
                sb.Append(i);
                sb.Append(i + 1 == fieldsCount ? " = " : ", ");
            }

            sb.Append(luaScript);

            var pScript = stringToPtr(sb.ToString());
            var pVarNames = new IAutoManagedMemory[fieldsCount];
            var pResult = m_buffer.Reserve(fieldsCount * 4);

            var vm = new LuaVMWrapper().PushStringPtr(pScript)
                                       .CallLuaFunction(runScript);

            for (var i = 0; i < fieldsCount; i++) {
                pVarNames[i] = stringToPtr($"v{i}");

                vm.GetText(pResult + i * 4, pVarNames[i]
                               .Start);
            }

            //await m_executor.ExecuteAsync(vm.CompileScript(m_assembler))
            //                .ConfigureAwait(false);

            var result = new List<string>(fieldsCount);

            for (var i = 0; i < fieldsCount; i++)
                result.Add(m_memory.ReadNullTerminatedString(pResult.Read<int>(i * 4), Encoding.UTF8));

            pScript.Dispose();

            foreach (var name in pVarNames)
                name.Dispose();

            pResult.Dispose();

            var t = new T();
            t.Parse(result.ToArray());

            return t;
        }

        // todo: rewrite
        public async Task<string> RunScriptAsync(string luaScript, string retVariableName) {
            const int runScript = 0x004DD490;
            var pScript = stringToPtr(luaScript);
            var pVarName = stringToPtr(retVariableName);
            var ppRes = m_buffer.Reserve(4);

            //await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pScript)
            //                                                .CallLuaFunction(runScript)
            //                                                .GetText(ppRes.Start, pVarName.Start)
            //                                                .CompileScript(m_assembler))
            //                .ConfigureAwait(false);

            pScript.Dispose();
            pVarName.Dispose();
            var res = m_memory.ReadNullTerminatedString(ppRes.Read<int>(0), Encoding.UTF8);
            ppRes.Dispose();

            return res;
        }

        public Task StartFollowUnitAsync(string unitMetaIdOrName) {
            return RunScriptAsync($"FollowUnit({unitMetaIdOrName})");
        }

        public Task<bool> UnitIsConnectedAsync(string unitMetaId) {
            return RunScriptAsync<LuaBoolean>($"UnitIsConnected({unitMetaId})", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> UnitIsDeadOrGhostAsync(string unitMetaId) {
            return RunScriptAsync<LuaBoolean>($"UnitIsDeadOrGhost({unitMetaId})", 1)
                .Cast<bool, LuaBoolean>();
        }

        public Task<bool> UnitIsPlayerAsync(string unitMetaId) {
            return RunScriptAsync<LuaBoolean>($"UnitIsPlayer({unitMetaId})", 1)
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