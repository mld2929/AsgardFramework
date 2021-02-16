using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.WoWAPI.Info;
using AsgardFramework.WoWAPI.Objects;
using System;
using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    internal class FunctionsAccessor : IFunctions
    {
        private readonly ICodeExecutor m_executor;
        private readonly IFasmCompiler m_compiler;
        internal FunctionsAccessor(ICodeExecutor executor, IFasmCompiler compiler)
        {
            m_executor = executor;
            m_compiler = compiler;
        }
        public async Task CastSpell(int spellId, ulong target)
        {
            const int c_castSpell = 0x0080DA40;
            var asm = new string[]
            {
                "push 0",
                "push 0",
                "push 0",
                "push 0",
                target.Push(),
                "push 0",
                "push " + spellId,
                "mov eax, " + c_castSpell,
                "call eax",
                "add esp, 0x20",
            };
            await m_executor.Execute(new LazyCodeBlock(m_compiler, asm));
        }

        public Task<LootSlotInfo> GetLootSlotInfo(int slotNumberFromOne) => throw new NotImplementedException();
        public Task<string> GetName(int objBase) => throw new NotImplementedException();
        public Task<int> GetNumLootItems() => throw new NotImplementedException();
        public Task<SpellInfo> GetSpellInfo(int spellId) => throw new NotImplementedException();
        public Task<SpellInfo> GetSpellInfo(string spellName) => throw new NotImplementedException();
        public Task Interact(int objBase) => throw new NotImplementedException();
        public Task LootSlot(int slotNumberFromOne) => throw new NotImplementedException();
        public Task SellItem(ulong itemGuid, ulong vendorGuid) => throw new NotImplementedException();
        public Task UpdatePosition(ObjectData obj) => throw new NotImplementedException();
    }
    internal static class LongHelper
    {
        internal static string Push(this ulong value) => unchecked($"push 0x{(int)(value >> 32):X}\npush 0x{(int)value:X}");
    }
}
