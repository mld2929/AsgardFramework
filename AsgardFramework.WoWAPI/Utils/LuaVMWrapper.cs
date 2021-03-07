using System.Collections.Generic;

using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Utils
{
    internal class LuaVMWrapper
    {
        #region Fields

        private readonly List<string> m_script = new List<string>();

        #endregion Fields

        #region Constructors

        internal LuaVMWrapper() {
            saveLuaStateOnStack();
        }

        #endregion Constructors

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
            m_script.Clear();
            saveLuaStateOnStack();

            return bytes.ToCodeBlock();
        }

        internal LuaVMWrapper GetText(int pBuffer, int pVar) {
            const int getText = 0x00819D40;

            m_script.AddRange(new[] {
                "push 0",
                "push -1",
                $"push {pVar}",
                getText.CallViaEax(),
                "add esp, 12"
            });

            return SaveValueTo(pBuffer);
        }

        internal LuaVMWrapper PushStringPtr(int stringPtr) {
            const int pushString = 0x0084E350;
            moveStateToEax();
            m_script.Add($"push {stringPtr}");
            m_script.Add("push eax");
            m_script.Add(pushString.CallViaEax());
            m_script.Add("add esp, 8");

            return this;
        }

        internal LuaVMWrapper PushStringPtr(IAutoManagedMemory buffer) {
            return PushStringPtr(buffer.Start);
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