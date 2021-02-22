using System;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IGameFunctions
    {
        #region Methods

        public Task AscendStopAsync() {
            throw new NotImplementedException();
        }

        public Task CastSpellAsync(int spellId, ulong target) {
            const int castSpell = 0x0080DA40;

            var asm = new[] {
                "push 0 0 0 0",
                target.Push(),
                "push 0 " + spellId,
                "mov eax, " + castSpell,
                "call eax",
                "add esp, 0x20"
            };

            return m_executor.ExecuteAsync(m_assembler.Assemble(asm)
                                                      .ToCodeBlock());
        }

        public Task ClickToMoveAsync(float x,
                                     float y,
                                     float z,
                                     int ctmState,
                                     ulong playerOrTargetGuid,
                                     float precision) {
            throw new NotImplementedException();
        }

        public Task<int> GetItemIdByNameAsync(string itemName) {
            throw new NotImplementedException();
        }

        public Task<string> GetNameAsync(int objBase) {
            throw new NotImplementedException();
        }

        public Task<string> GetPlayerNameAsync() {
            throw new NotImplementedException();
        }

        public async Task<Position> GetPositionAsync(int objBase) {
            const int getPositionOffset = 12 * 4;
            const int size = sizeof(float) * 4;
            var getPosition = m_memory.Read<int>(m_memory.Read<int>(objBase) + getPositionOffset);

            if (!m_buffer.TryReserve(size, out var buffer))
                throw new InvalidOperationException("Can't reserve memory");

            var asm = new[] {
                $"mov ecx, {objBase}", // this
                $"mov eax, {getPosition}",
                $"push {buffer.Start}",
                "call eax"
            };

            await m_executor.ExecuteAsync(m_assembler.Assemble(asm)
                                                     .ToCodeBlock())
                            .ConfigureAwait(false);

            var result = m_memory.Read<Position>(buffer.Start);
            buffer.Dispose();

            return result;
        }

        public Task<bool> HasAuraBySpellIdAsync(int objBase, int spellId) {
            throw new NotImplementedException();
        }

        public Task InteractAsync(int objBase) {
            throw new NotImplementedException();
        }

        public Task JumpOrAscendStartAsync() {
            throw new NotImplementedException();
        }

        public Task MoveBackwardStartAsync() {
            throw new NotImplementedException();
        }

        public Task MoveBackwardStopAsync() {
            throw new NotImplementedException();
        }

        public Task MoveForwardStartAsync() {
            throw new NotImplementedException();
        }

        public Task MoveForwardStopAsync() {
            throw new NotImplementedException();
        }

        public Task ObjectOnClickAsync(int objBase) {
            throw new NotImplementedException();
        }

        public Task SellItemAsync(ulong itemGuid, ulong vendorGuid) {
            throw new NotImplementedException();
        }

        public Task StartEnterWorldAsync() {
            throw new NotImplementedException();
        }

        public Task StartLoginToDefaultServerAsync(string login, string password) {
            throw new NotImplementedException();
        }

        public Task TargetUnitAsync(ulong guid) {
            throw new NotImplementedException();
        }

        public Task TurnLeftStartAsync() {
            throw new NotImplementedException();
        }

        public Task TurnLeftStopAsync() {
            throw new NotImplementedException();
        }

        public Task TurnRightStartAsync() {
            throw new NotImplementedException();
        }

        public Task TurnRightStopAsync() {
            throw new NotImplementedException();
        }

        public Task UnitOnClickAsync(int unitBase) {
            throw new NotImplementedException();
        }

        public Task UseItemAsync(int itemBase, ulong itemGuid) {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}