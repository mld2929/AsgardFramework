using System;
using System.Linq;
using System.Threading.Tasks;

using AsgardFramework.CodeInject;
using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IGameFunctions
    {
        public Task AscendStop() {
            throw new NotImplementedException();
        }

        public async Task CastSpell(int spellId, ulong target) {
            const int c_castSpell = 0x0080DA40;
            var asm = new string[]
            {
                "push 0 0 0 0",
                target.Push(),
                "push 0 " + spellId,
                "mov eax, " + c_castSpell,
                "call eax",
                "add esp, 0x20",
            };
            await m_executor.Execute(new CompiledCodeBlock(m_compiler.Assemble(asm).ToArray()));
        }

        public Task ClickToMove(float x, float y, float z, int ctmState, ulong playerOrTargetGuid, float precision) {
            throw new NotImplementedException();
        }

        public Task<int> GetItemIdByName(string itemName) {
            throw new NotImplementedException();
        }

        public Task<string> GetName(int objBase) {
            throw new NotImplementedException();
        }

        public Task<string> GetPlayerName() {
            throw new NotImplementedException();
        }

        public async Task<Position> GetPosition(int objBase) {
            const int c_getPositionOffset = 0x12;
            var getPosition = m_memory.Read(m_memory.Read(objBase, 4).ToInt32() + c_getPositionOffset, 4).ToInt32();
            var size = sizeof(float) * 4;
            if (!m_buffer.TryReserve(size, out var buffer)) {
                throw new OutOfMemoryException();
            }

            var asm = new string[] {
                $"mov ecx, {objBase}", // this
                $"mov eax, {getPosition}",
                $"push {buffer.Start}",
                "call eax"
            };
            await m_executor.Execute(new CompiledCodeBlock(m_compiler.Assemble(asm).ToArray()));
            var result = m_memory.Read(buffer.Start, size).ToArrayOfFloat();
            buffer.Dispose();
            return new Position(result[0], result[1], result[2], result[3]);
        }

        public Task<bool> HasAuraBySpellId(int objBase, int spellId) {
            throw new NotImplementedException();
        }

        public Task Interact(int objBase) {
            throw new NotImplementedException();
        }

        public Task JumpOrAscendStart() {
            throw new NotImplementedException();
        }

        public Task MoveBackwardStart() {
            throw new NotImplementedException();
        }

        public Task MoveBackwardStop() {
            throw new NotImplementedException();
        }

        public Task MoveForwardStart() {
            throw new NotImplementedException();
        }

        public Task MoveForwardStop() {
            throw new NotImplementedException();
        }

        public Task ObjectOnClick(int objBase) {
            throw new NotImplementedException();
        }

        public Task SellItem(ulong itemGuid, ulong vendorGuid) {
            throw new NotImplementedException();
        }

        public Task StartEnterWorld() {
            throw new NotImplementedException();
        }

        public Task StartLoginToDefaultServer(string login, string password) {
            throw new NotImplementedException();
        }

        public Task Target(ulong guid) {
            throw new NotImplementedException();
        }

        public Task TurnLeftStart() {
            throw new NotImplementedException();
        }

        public Task TurnLeftStop() {
            throw new NotImplementedException();
        }

        public Task TurnRightStart() {
            throw new NotImplementedException();
        }

        public Task TurnRightStop() {
            throw new NotImplementedException();
        }

        public Task UnitOnClick(int unitBase) {
            throw new NotImplementedException();
        }

        public Task UseItem(int itemBase, ulong itemGuid) {
            throw new NotImplementedException();
        }
    }
}
