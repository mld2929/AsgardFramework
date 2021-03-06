using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Info;
using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IAggregatedFunctions
    {
        public Task<IEnumerable<LootSlotInfo>> GetLootSlotsInfoAsync(Range range) {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetNamesAsync(IEnumerable<int> objBases) {
            const int getNameOffset = 54 * 4;
            var count = objBases.Count();
            var size = count * 4;
            var asm = new List<string>();

            var buffer = m_buffer.Reserve(size);
            var current = buffer.Start;

            foreach (var objBase in objBases) {
                var getName = m_memory.Read<int>(m_memory.Read<int>(objBase) + getNameOffset);

                asm.AddRange(new[] {
                    $"mov ecx, {objBase}", // this
                    getName.CallViaEax(),
                    $"mov dword [{current}], eax"
                });

                current += 4;
            }

            await m_executor.ExecuteAsync(m_assembler.Assemble(asm)
                                                     .ToCodeBlock())
                            .ConfigureAwait(false);

            var result = new List<string>(count);
            current = buffer.Start;

            for (var i = 0; i < count; i++, current += 4)
                result.Add(m_memory.ReadNullTerminatedString(m_memory.Read<int>(current), Encoding.UTF8));

            buffer.Dispose();

            return result;
        }

        public async Task<IEnumerable<Position>> GetPositionsAsync(IEnumerable<int> objBases) {
            const int getPositionOffset = 12 * 4;
            var count = objBases.Count();
            var size = sizeof(float) * 4 * count;
            var asm = new List<string>();

            var buffer = m_buffer.Reserve(size);
            var current = buffer.Start;

            foreach (var objBase in objBases) {
                var getPosition = m_memory.Read<int>(m_memory.Read<int>(objBase) + getPositionOffset);

                asm.AddRange(new[] {
                    $"mov ecx, {objBase}", // this
                    $"mov eax, {getPosition}",
                    $"push {current}",
                    "call eax"
                });

                current += sizeof(float) * 4;
            }

            await m_executor.ExecuteAsync(m_assembler.Assemble(asm)
                                                     .ToCodeBlock())
                            .ConfigureAwait(false);

            var result = new List<Position>(count);
            current = buffer.Start;

            for (var i = 0; i < count; i++, current += sizeof(float) * 4)
                result.Add(m_memory.Read<Position>(current));

            buffer.Dispose();

            return result;
        }

        public Task<IEnumerable<SpellCooldownInfo>> GetSpellsCooldownAsync(IEnumerable<int> ids) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SpellInfo>> GetSpellsInfoAsync(IEnumerable<int> ids) {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UnitAuraInfo>> GetUnitAurasInfoAsync(string unitName, Range range, string filter) {
            throw new NotImplementedException();
        }
    }
}