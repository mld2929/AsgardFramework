using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal static class AsmHelper
    {
        #region Methods

        /// <summary>
        ///     Safely calls address, writing it in eax register
        /// </summary>
        /// <remarks>
        ///     Don't know why, but <c>call dword [0xDEADBEEF]</c> doesn't work correctly
        /// </remarks>
        internal static string CallViaEax(this int value) {
            return $"mov eax, {value}\ncall eax";
        }

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