using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor
    {
        #region Constructors

        internal FunctionsAccessor(EndSceneHookExecutor executor, IGlobalMemory memory) {
            m_executor = executor;
            m_memory = memory;
            m_buffer = m_memory.AllocateAutoScalingShared(c_dataBufferSize);
        }

        #endregion Constructors

        #region Fields

        private const int c_dataBufferSize = 0x800000;

        private readonly IAutoManagedSharedBuffer m_buffer;

        private readonly EndSceneHookExecutor m_executor;

        private readonly IGlobalMemory m_memory;

        #endregion Fields
    }
}