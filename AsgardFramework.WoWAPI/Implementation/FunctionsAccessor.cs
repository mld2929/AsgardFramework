using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor
    {
        #region Constructors

        internal FunctionsAccessor(ICodeExecutor executor, IFasmAssembler assembler, IGlobalMemory memory) {
            m_executor = executor;
            m_assembler = assembler;
            m_memory = memory;
            m_buffer = m_memory.AllocateAutoScalingShared(c_dataBufferSize);
        }

        #endregion Constructors

        #region Fields

        private const int c_dataBufferSize = 0x800000; // 800 KB

        private readonly IFasmAssembler m_assembler;

        
        private readonly IAutoManagedSharedBuffer m_buffer;

        private readonly ICodeExecutor m_executor;

        private readonly IGlobalMemory m_memory;

        #endregion Fields
    }
}