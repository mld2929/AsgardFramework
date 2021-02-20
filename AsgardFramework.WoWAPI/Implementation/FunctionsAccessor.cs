using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal static class LongHelper
    {
        #region Methods

        internal static string Push(this ulong value) {
            return unchecked($"push 0x{(int)(value >> 32):X} 0x{(int)value:X}");
        }

        #endregion Methods
    }

    internal partial class FunctionsAccessor
    {
        #region Constructors

        internal FunctionsAccessor(ICodeExecutor executor, IFasmAssembler compiler, IGlobalMemory memory) {
            m_executor = executor;
            m_compiler = compiler;
            m_memory = memory;
            m_buffer = m_memory.AllocateAutoScalingShared(c_dataBufferSize);
        }

        #endregion Constructors

        #region Fields

        private const int c_dataBufferSize = 0x800000; // 800 KB
        private readonly IAutoManagedSharedBuffer m_buffer;
        private readonly IFasmAssembler m_compiler;
        private readonly ICodeExecutor m_executor;
        private readonly IGlobalMemory m_memory;

        #endregion Fields
    }
}