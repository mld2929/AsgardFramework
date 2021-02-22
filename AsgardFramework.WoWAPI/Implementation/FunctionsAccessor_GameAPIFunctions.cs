using System;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Info;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IGameAPIFunctions
    {
        #region Methods

        public Task AttackTargetAsync() {
            throw new NotImplementedException();
        }

        public Task DismountAsync() {
            const int dismount = 0x0051D170;
            var bytes = m_assembler.Assemble(dismount.CallViaEax());

            return m_executor.ExecuteAsync(bytes.ToCodeBlock());
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
            return getSpellInfoImplAsync(new LuaVMWrapper().PushInt(spellId));
        }

        public async Task<SpellInfo> GetSpellInfoAsync(string spellName) {
            var ptr = stringToPtr(spellName);

            var info = await getSpellInfoImplAsync(new LuaVMWrapper().PushStringPtr(ptr.Start))
                           .ConfigureAwait(false);

            ptr.Dispose();

            return info;
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

        public Task<UnitFactionGroupInfo> GetUnitFactionGroupAsync(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        public Task<bool> HasPlayerFullControlAsync() {
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

        public Task<bool> IsPlayerFallingAsync() {
            throw new NotImplementedException();
        }

        public Task<bool> IsUnitAffectingCombatAsync(string unitMetaId) {
            throw new NotImplementedException();
        }

        public Task LootSlotAsync(int slotNumberFromOne) {
            const int lootSlot = 0x00589520;
            var script = new LuaVMWrapper();

            var compiled = script.PushInt(slotNumberFromOne)
                                 .CallLuaFunction(lootSlot)
                                 .CompileScript(m_assembler);

            return m_executor.ExecuteAsync(compiled);
        }

        public Task RunScriptAsync(string luaScript) {
            throw new NotImplementedException();
        }

        public Task StartFollowUnitAsync(string unitMetaIdOrName) {
            throw new NotImplementedException();
        }

        public Task<bool> UnitIsPlayerAsync(string unitMetaId) {
            throw new NotImplementedException();
        }

        private async Task<SpellInfo> getSpellInfoImplAsync(LuaVMWrapper withPushedArgument) {
            const int getSpellInfo = 0x00540A30;
            const int spellInfoSize = 48;

            var spellInfo = m_buffer.Reserve(spellInfoSize);

            var script = withPushedArgument.CallLuaFunction(getSpellInfo)
                                           .PopStringStr(spellInfo) // Name
                                           .PopStringStr(spellInfo) // Rank or secondary
                                           .PopStringStr(spellInfo) // Icon
                                           .PopNumber(spellInfo)    // Cost
                                           .PopInteger(spellInfo)   // IsFunnel
                                           .PopInteger(spellInfo)   // Power type
                                           .PopInteger(spellInfo)   // Cast time (milliseconds)
                                           .PopNumber(spellInfo)    // Min range
                                           .PopNumber(spellInfo)    // Max range
                                           .CompileScript(m_assembler);

            await m_executor.ExecuteAsync(script)
                            .ConfigureAwait(false);

            var result = new SpellInfo(spellInfo.Read<SpellInfoRaw>(0), m_memory);
            spellInfo.Dispose();

            return result;
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