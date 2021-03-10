using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor
    {
        #region Constructors

        internal FunctionsAccessor(IMainThreadExecutor executor, IGlobalMemory memory, ILuaScriptExecutor scriptExecutor) {
            m_executor = executor;
            m_memory = memory;
            m_luaExecutor = scriptExecutor;
            m_buffer = m_memory.AllocateAutoScalingShared(c_dataBufferSize);
        }

        #endregion Constructors

        #region Fields

        private const int c_dataBufferSize = 0x800000;

        private readonly IAutoManagedSharedBuffer m_buffer;

        private readonly IMainThreadExecutor m_executor;

        private readonly ILuaScriptExecutor m_luaExecutor;

        private readonly IGlobalMemory m_memory;

        #endregion Fields
    }
}