using System.Collections.Generic;
using System.Linq;

using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.WoWAPI.Implementation;

namespace AsgardFramework.WoWAPI.Utils
{
    internal class LuaVMWrapper
    {
        #region Constructors

        internal LuaVMWrapper() {
            saveLuaStateOnStack();
        }

        #endregion Constructors

        #region Fields

        private readonly List<string> m_script = new List<string>();
        private int m_columnState;

        #endregion Fields

        #region Methods

        internal LuaVMWrapper CallLuaFunction(int address) {
            moveStateToEax();
            m_script.Add("push eax");
            m_script.Add(address.CallViaEax());
            m_script.Add("add esp, 4");

            return this;
        }

        internal ICodeBlock CompileScript(IFasmAssembler assembler) {
            const int setTop = 0x0084DBF0;
            moveStateToEax();
            m_script.Add("push 0");
            m_script.Add("push eax");
            m_script.Add(setTop.CallViaEax());
            m_script.Add("add esp, 12");
            var bytes = assembler.Assemble(m_script);
            m_columnState = 0;
            m_script.Clear();
            saveLuaStateOnStack();

            return new CompiledCodeBlock(bytes.ToArray());
        }

        internal LuaVMWrapper PopInteger() {
            const int popInteger = 0x0084E070;
            moveStateToEax();
            m_script.Add($"push {m_columnState}");
            m_script.Add("push eax");
            m_script.Add(popInteger.CallViaEax());
            m_script.Add("add esp, 8");
            m_columnState++;

            return this;
        }

        internal LuaVMWrapper PopStringPtr() {
            const int popString = 0x0084E0E0;
            moveStateToEax();
            m_script.Add($"push 0 {m_columnState}");
            m_script.Add("push eax");
            m_script.Add(popString.CallViaEax());
            m_script.Add("add esp, 12");
            m_columnState++;

            return this;
        }

        internal LuaVMWrapper PushInt(int value) {
            const int pushInteger = 0x0084E2D0;
            moveStateToEax();
            m_script.Add($"push {value}");
            m_script.Add("push eax");
            m_script.Add(pushInteger.CallViaEax());
            m_script.Add("add esp, 8");
            m_columnState++;

            return this;
        }

        internal LuaVMWrapper PushStringPtr(int stringPtr) {
            const int pushString = 0x0084E350;
            moveStateToEax();
            m_script.Add($"push {stringPtr}");
            m_script.Add("push eax");
            m_script.Add(pushString.CallViaEax());
            m_script.Add("add esp, 8");
            m_columnState++;

            return this;
        }

        internal LuaVMWrapper SaveValueTo(int pBuffer) {
            m_script.Add($"mov dword [{pBuffer}], eax");

            return this;
        }

        private void moveStateToEax() {
            m_script.Add("mov eax, [esp]");
        }

        private void saveLuaStateOnStack() {
            const int getState = 0x00817DB0;
            m_script.Add(getState.CallViaEax());
            m_script.Add("push eax");
        }

        #endregion Methods
    }
}