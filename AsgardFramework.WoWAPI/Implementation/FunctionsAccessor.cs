using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor
    {
        private const int c_dataBufferSize = 8096;
        private readonly ICodeExecutor m_executor;
        private readonly IFasmAssembler m_compiler;
        private readonly IGlobalMemory m_memory;
        private readonly IAutoManagedSharedBuffer m_buffer;
        internal FunctionsAccessor(ICodeExecutor executor, IFasmAssembler compiler, IGlobalMemory memory) {
            m_executor = executor;
            m_compiler = compiler;
            m_memory = memory;
            m_buffer = m_memory.AllocateAutoScalingShared(c_dataBufferSize);
        }
    }
    internal static class LongHelper
    {
        internal static string Push(this ulong value) {
            return unchecked($"push 0x{(int)(value >> 32):X} 0x{(int)value:X}");
        }
    }
}
