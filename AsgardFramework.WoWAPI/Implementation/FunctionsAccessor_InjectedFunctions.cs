using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IInjectedFunctions
    {
        private readonly List<IAutoManagedMemory> m_gcSafe = new List<IAutoManagedMemory>();

        #region Methods

        public Task StartExecuteScriptAtEachFrameAsync(string luaScript) {
            const int runScript = 0x004DD490;
            var pScript = m_memory.Allocate(Encoding.UTF8.GetByteCount(luaScript) + 1);
            pScript.WriteNullTerminatedString(0, luaScript, Encoding.UTF8);
            m_gcSafe.Add(pScript);

            return m_executor.StartExecutePermanentlyAsync(new LuaVMWrapper().PushStringPtr(pScript)
                                                                             .CallLuaFunction(runScript)
                                                                             .CompileScript(m_assembler));
        }

        public bool AntiAFK {
            set => ((EndSceneHookExecutor)m_executor).AntiAFK = value; // yeah type cast 
        }

        public void DisableWarden() {
            m_memory.LoadDll($"{AppDomain.CurrentDomain.BaseDirectory}/AsgardFramework.WardenDefuser.dll");
        }

        #endregion Methods
    }
}