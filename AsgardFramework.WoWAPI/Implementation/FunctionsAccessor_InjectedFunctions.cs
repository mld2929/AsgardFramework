using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IInjectedFunctions
    {
        #region Fields

        private readonly List<IAutoManagedMemory> m_gcSafe = new List<IAutoManagedMemory>();

        #endregion Fields

        #region Properties

        // todo: rewrite
        public bool AntiAFK {
            get;
            set; //((EndSceneHookExecutor)m_executor).AntiAFK = value; // yeah type cast
        }

        #endregion Properties

        #region Methods

        // todo: rewrite
        public void DisableWarden() {
            //m_memory.LoadDllAsync($"{AppDomain.CurrentDomain.BaseDirectory}/AsgardFramework.Core.dll");
        }

        // todo: rewrite
        public async Task StartExecuteScriptAtEachFrameAsync(string luaScript) {
            const int runScript = 0x004DD490;
            var pScript = m_memory.Allocate(Encoding.UTF8.GetByteCount(luaScript) + 1);
            pScript.WriteNullTerminatedString(0, luaScript, Encoding.UTF8);
            m_gcSafe.Add(pScript);

            //return m_executor.StartExecutePermanentlyAsync(new LuaVMWrapper().PushStringPtr(pScript)
            //                                                                 .CallLuaFunction(runScript)
            //                                                                 .CompileScript(m_assembler));
        }

        public void Teleport(int playerBase, float x, float y, float z) {
            const int pos_x = 0x79C;
            const int pos_y = 0x798;
            const int pos_z = 0x7A0;
            m_memory.Write(playerBase + pos_x, x);
            m_memory.Write(playerBase + pos_y, y);
            m_memory.Write(playerBase + pos_z, z);
        }

        #endregion Methods
    }
}