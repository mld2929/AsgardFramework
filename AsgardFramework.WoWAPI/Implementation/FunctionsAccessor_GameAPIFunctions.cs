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
            const int attackTarget = 0x0051A650;
            return m_executor.ExecuteAsync(new LuaVMWrapper().CallLuaFunction(attackTarget).CompileScript(m_assembler));
        }

        public Task DismountAsync() {
            const int dismount = 0x0051D170;
            var bytes = m_assembler.Assemble(dismount.CallViaEax());

            return m_executor.ExecuteAsync(bytes.ToCodeBlock());
        }

        public async Task EquipItemAsync(string name) {
            const int equipItem = 0x0051CDB0;
            var pName = stringToPtr(name);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pName)
                                                            .CallLuaFunction(equipItem)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);
            pName.Dispose();
        }

        public Task EquipItemAsync(int itemId) {
            const int equipItem = 0x0051CDB0;

            return m_executor.ExecuteAsync(new LuaVMWrapper().PushInt(itemId)
                                                             .CallLuaFunction(equipItem)
                                                             .CompileScript(m_assembler));
        }

        public async Task<ItemSpellInfo> GetItemSpellAsync(string itemName) {
            const int getItemSpell = 0x00517100;
            const int itemSpellInfoSize = 8;
            var pName = stringToPtr(itemName);
            var itemSpellInfo = m_buffer.Reserve(itemSpellInfoSize);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pName)
                                                            .CallLuaFunction(getItemSpell)
                                                            .PopStringPtr(itemSpellInfo)
                                                            .PopStringPtr(itemSpellInfo)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new ItemSpellInfo(itemSpellInfo.Read<ItemSpellInfoRaw>(0), m_memory);
            pName.Dispose();
            itemSpellInfo.Dispose();

            return result;
        }

        public async Task<ItemSpellInfo> GetItemSpellAsync(int itemId) {
            const int getItemSpell = 0x00517100;
            const int itemSpellInfoSize = 8;
            var itemSpellInfo = m_buffer.Reserve(itemSpellInfoSize);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushInt(itemId)
                                                            .CallLuaFunction(getItemSpell)
                                                            .PopStringPtr(itemSpellInfo)
                                                            .PopStringPtr(itemSpellInfo)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new ItemSpellInfo(itemSpellInfo.Read<ItemSpellInfoRaw>(0), m_memory);
            itemSpellInfo.Dispose();

            return result;
        }

        public async Task<LootSlotInfo> GetLootSlotInfoAsync(int slotNumberFromOne) {
            const int getLootSlotInfo = 0x00588570;
            const int lootSlotInfoSize = 20;
            var lootSlotInfo = m_buffer.Reserve(lootSlotInfoSize);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushInt(slotNumberFromOne)
                                                            .CallLuaFunction(getLootSlotInfo)
                                                            .PopStringPtr(lootSlotInfo)
                                                            .PopStringPtr(lootSlotInfo)
                                                            .PopInteger(lootSlotInfo)
                                                            .PopInteger(lootSlotInfo)
                                                            .PopInteger(lootSlotInfo)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new LootSlotInfo(lootSlotInfo.Read<LootSlotInfoRaw>(0), m_memory);
            lootSlotInfo.Dispose();

            return result;
        }

        public Task<int> GetNumLootItemsAsync() {
            const int getNumLootItems = 0x00588540;

            return runAndGetInt(getNumLootItems);
        }

        public async Task<SpellCooldownInfo> GetSpellCooldownAsync(string name) {
            const int getSpellCooldown = 0x00540E80;
            const int spellCooldownSize = 20;
            var spellCooldown = m_buffer.Reserve(spellCooldownSize);
            var pName = stringToPtr(name);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pName)
                                                            .CallLuaFunction(getSpellCooldown)
                                                            .PopNumber(spellCooldown)
                                                            .PopNumber(spellCooldown)
                                                            .PopInteger(spellCooldown)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = spellCooldown.Read<SpellCooldownInfo>(0);
            spellCooldown.Dispose();
            pName.Dispose();

            return result;
        }

        public async Task<SpellCooldownInfo> GetSpellCooldownAsync(int spellId) {
            const int getSpellCooldown = 0x00540E80;
            const int spellCooldownSize = 20;
            var spellCooldown = m_buffer.Reserve(spellCooldownSize);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushInt(spellId)
                                                            .CallLuaFunction(getSpellCooldown)
                                                            .PopNumber(spellCooldown)
                                                            .PopNumber(spellCooldown)
                                                            .PopInteger(spellCooldown)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = spellCooldown.Read<SpellCooldownInfo>(0);
            spellCooldown.Dispose();

            return result;
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

        public async Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, int index, string filter = null) {
            const int unitAura = 0x00614D40;
            const int unitAuraInfoSize = 52;
            var pMeta = stringToPtr(unitMetaId);
            var pFilter = filter != null ? stringToPtr(filter) : null;
            var unitAuraInfo = m_buffer.Reserve(unitAuraInfoSize);
            var script = new LuaVMWrapper().PushStringPtr(pMeta)
                                           .PushInt(index);

            if (filter != null)
                script = script.PushStringPtr(pFilter);

            await m_executor.ExecuteAsync(script.CallLuaFunction(unitAura)
                                                .PopStringPtr(unitAuraInfo)
                                                .PopStringPtr(unitAuraInfo)
                                                .PopStringPtr(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopNumber(unitAuraInfo)
                                                .PopNumber(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new UnitAuraInfo(unitAuraInfo.Read<UnitAuraInfoRaw>(0), m_memory);
            pMeta.Dispose();
            pFilter?.Dispose();
            unitAuraInfo.Dispose();

            return result;
        }

        public async Task<UnitAuraInfo> GetUnitAuraAsync(string unitMetaId, string auraName, string auraSecondaryText = null, string filter = null) {
            const int unitAura = 0x00614D40;
            const int unitAuraInfoSize = 52;
            var pMeta = stringToPtr(unitMetaId);
            var pFilter = filter != null ? stringToPtr(filter) : null;
            var pAuraName = stringToPtr(auraName);
            var pSecondary = auraSecondaryText != null ? stringToPtr(auraSecondaryText) : null;
            var unitAuraInfo = m_buffer.Reserve(unitAuraInfoSize);
            var script = new LuaVMWrapper().PushStringPtr(pMeta)
                                           .PushStringPtr(pAuraName);
            if (auraSecondaryText != null)
                script = script.PushStringPtr(pSecondary);
            if (filter != null)
                script = script.PushStringPtr(pFilter);

            await m_executor.ExecuteAsync(script.CallLuaFunction(unitAura)
                                                .PopStringPtr(unitAuraInfo)
                                                .PopStringPtr(unitAuraInfo)
                                                .PopStringPtr(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopNumber(unitAuraInfo)
                                                .PopNumber(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .PopInteger(unitAuraInfo)
                                                .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new UnitAuraInfo(unitAuraInfo.Read<UnitAuraInfoRaw>(0), m_memory);
            pMeta.Dispose();
            pFilter?.Dispose();
            pSecondary?.Dispose();
            pAuraName.Dispose();
            unitAuraInfo.Dispose();

            return result;
        }

        public async Task<UnitClassInfo> GetUnitClassAsync(string unitMetaIdOrName) {
            const int unitClass = 0x0060FEC0;
            const int unitClassInfoSize = 8;
            var pMeta = stringToPtr(unitMetaIdOrName);
            var unitClassInfo = m_buffer.Reserve(unitClassInfoSize);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pMeta)
                                                            .CallLuaFunction(unitClass)
                                                            .PopStringPtr(unitClassInfo)
                                                            .PopStringPtr(unitClassInfo)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new UnitClassInfo(unitClassInfo.Read<UnitClassInfoRaw>(0), m_memory);
            pMeta.Dispose();
            unitClassInfo.Dispose();

            return result;
        }

        public async Task<UnitFactionGroupInfo> GetUnitFactionGroupAsync(string unitMetaIdOrName) {
            const int unitFactionGroup = 0x0060D0A0;
            const int unitFactionGroupInfoSize = 8;
            var pMeta = stringToPtr(unitMetaIdOrName);
            var unitFactionGroupInfo = m_buffer.Reserve(unitFactionGroupInfoSize);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pMeta)
                                                            .CallLuaFunction(unitFactionGroup)
                                                            .PopStringPtr(unitFactionGroupInfo)
                                                            .PopStringPtr(unitFactionGroupInfo)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = new UnitFactionGroupInfo(unitFactionGroupInfo.Read<UnitFactionGroupInfoRaw>(0), m_memory);
            pMeta.Dispose();
            unitFactionGroupInfo.Dispose();

            return result;
        }

        public Task<bool> HasPlayerFullControlAsync() {
            const int hasFullControl = 0x00611600;

            return runAndGetBool(hasFullControl);
        }

        public Task<bool> IsLoggedInAsync() {
            const int isLoggedIn = 0x0060A450;

            return runAndGetBool(isLoggedIn);
        }

        public Task<bool> IsMountedAsync() {
            const int isMounted = 0x006125A0;

            return runAndGetBool(isMounted);
        }

        public Task<bool> IsOutdoorsAsync() {
            const int isOutdoors = 0x00612360;
            return runAndGetBool(isOutdoors);
        }

        public Task<bool> IsPlayerFallingAsync() {
            const int isFalling = 0x00612430;

            return runAndGetBool(isFalling);
        }

        public async Task<bool> IsUnitAffectingCombatAsync(string unitMetaId) {
            const int unitAffectingCombat = 0x0060F860;
            var buffer = m_buffer.Reserve(4);
            var pMeta = stringToPtr(unitMetaId);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pMeta)
                                                            .CallLuaFunction(unitAffectingCombat)
                                                            .PopInteger(buffer)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = buffer.Read<int>(0);
            pMeta.Dispose();
            buffer.Dispose();
            return result != 0;
        }

        public Task LootSlotAsync(int slotNumberFromOne) {
            const int lootSlot = 0x00589520;
            var script = new LuaVMWrapper();

            var compiled = script.PushInt(slotNumberFromOne)
                                 .CallLuaFunction(lootSlot)
                                 .CompileScript(m_assembler);

            return m_executor.ExecuteAsync(compiled);
        }

        public async Task RunScriptAsync(string luaScript) {
            const int runScript = 0x004DD490;
            var pScript = stringToPtr(luaScript);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pScript)
                                                            .CallLuaFunction(runScript)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);
            
            pScript.Dispose();
        }

        public async Task StartFollowUnitAsync(string unitMetaIdOrName) {
            const int followUnit = 0x005224C0;
            var pMeta = stringToPtr(unitMetaIdOrName);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pMeta)
                                                            .CallLuaFunction(followUnit)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);
            
            pMeta.Dispose();
        }

        public async Task<bool> UnitIsPlayerAsync(string unitMetaId) {
            const int unitIsPlayer = 0x0060C4B0;
            var buffer = m_buffer.Reserve(4);
            var pMeta = stringToPtr(unitMetaId);

            await m_executor.ExecuteAsync(new LuaVMWrapper().PushStringPtr(pMeta)
                                                            .CallLuaFunction(unitIsPlayer)
                                                            .PopInteger(buffer)
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = buffer.Read<int>(0);
            pMeta.Dispose();
            buffer.Dispose();
            return result != 0;
        }

        private async Task<SpellInfo> getSpellInfoImplAsync(LuaVMWrapper withPushedArgument) {
            const int getSpellInfo = 0x00540A30;
            const int spellInfoSize = 48;

            var spellInfo = m_buffer.Reserve(spellInfoSize);

            var script = withPushedArgument.CallLuaFunction(getSpellInfo)
                                           .PopStringPtr(spellInfo) // Name
                                           .PopStringPtr(spellInfo) // Rank or secondary
                                           .PopStringPtr(spellInfo) // Icon
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

        private async Task<int> runAndGetInt(int luaFunction) {
            var buffer = m_buffer.Reserve(4);

            await m_executor.ExecuteAsync(new LuaVMWrapper().CallLuaFunction(luaFunction)
                                                            .PopInteger()
                                                            .CompileScript(m_assembler))
                            .ConfigureAwait(false);

            var result = buffer.Read<int>(0);
            buffer.Dispose();

            return result;
        }

        private async Task<bool> runAndGetBool(int luaFunction) {
            return (await runAndGetInt(luaFunction)
                        .ConfigureAwait(false)) !=
                   0;
        }

        #endregion Methods
    }
}