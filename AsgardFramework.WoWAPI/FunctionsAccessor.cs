using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Info;
using AsgardFramework.WoWAPI.Objects;
using System;
using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    internal class FunctionsAccessor : IFunctions
    {
        private const int c_dataBufferSize = 8096;
        private readonly ICodeExecutor m_executor;
        private readonly IFasmCompiler m_compiler;
        private readonly IGlobalMemory m_memory;
        private readonly IAutoManagedSharedBuffer m_buffer;
        internal FunctionsAccessor(ICodeExecutor executor, IFasmCompiler compiler, IGlobalMemory memory) {
            m_executor = executor;
            m_compiler = compiler;
            m_memory = memory;
            m_buffer = m_memory.AllocateAutoScalingShared(c_dataBufferSize);
        }
        public async Task CastSpell(int spellId, ulong target) {
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

        public Task<LootSlotInfo> GetLootSlotInfo(int slotNumberFromOne) {
            throw new NotImplementedException();
        }

        public Task<string> GetName(int objBase) {
            throw new NotImplementedException();
        }

        public Task<int> GetNumLootItems() {
            throw new NotImplementedException();
        }

        public Task<SpellInfo> GetSpellInfo(int spellId) {
            throw new NotImplementedException();
        }

        public Task<SpellInfo> GetSpellInfo(string spellName) {
            throw new NotImplementedException();
        }

        public Task Interact(int objBase) {
            throw new NotImplementedException();
        }

        public Task LootSlot(int slotNumberFromOne) {
            throw new NotImplementedException();
        }

        public Task SellItem(ulong itemGuid, ulong vendorGuid) {
            throw new NotImplementedException();
        }

        public async Task UpdatePosition(ObjectData obj) {
            const int c_getPositionOffset = 0x12;
            var getPosition = m_memory.Read(obj.Common.VFTable + c_getPositionOffset, 4).ToInt32();
            var size = sizeof(float) * 4;
            if (!m_buffer.TryReserve(size, out var buffer)) {
                throw new OutOfMemoryException();
            }

            var asm = new string[] {
                $"mov ecx, {obj.Base}", // this
                $"mov eax, {getPosition}",
                $"push {buffer.Start}",
                "call eax"
            };
            await m_executor.Execute(new LazyCodeBlock(m_compiler, asm));
            var result = m_memory.Read(buffer.Start, size).ToArrayOfFloat();
            buffer.Dispose();
            obj.Position = new Position(result[0], result[1], result[2], result[3]);
        }
    }
    internal static class LongHelper
    {
        internal static string Push(this ulong value) {
            return unchecked($"push 0x{(int)(value >> 32):X}\npush 0x{(int)value:X}");
        }
    }
}
