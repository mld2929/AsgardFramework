using System.Collections.Generic;

using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

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

        private int m_bufferOffset;

        private int m_columnState = 1;

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
            m_columnState = 1;
            m_script.Clear();
            saveLuaStateOnStack();

            return bytes.ToCodeBlock();
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

        internal LuaVMWrapper PopInteger(int pBuffer) {
            return PopInteger()
                .SaveValueTo(pBuffer);
        }

        internal LuaVMWrapper PopInteger(IAutoManagedMemory buffer) {
            return PopInteger(buffer + m_bufferOffset);
        }

        internal LuaVMWrapper PopNumber(int pBuffer) {
            return PopNumber()
                .SaveNumberTo(pBuffer);
        }

        internal LuaVMWrapper PopNumber(IAutoManagedMemory buffer) {
            return PopNumber(buffer + m_bufferOffset);
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

        internal LuaVMWrapper PopStringPtr(int pBuffer) {
            return PopStringPtr()
                .SaveValueTo(pBuffer);
        }

        internal LuaVMWrapper PopStringPtr(IAutoManagedMemory buffer) {
            return PopStringPtr(buffer + m_bufferOffset);
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

        internal LuaVMWrapper PushStringPtr(IAutoManagedMemory buffer) {
            return PushStringPtr(buffer.Start);
        }

        internal LuaVMWrapper SaveValueTo(int pBuffer) {
            m_script.Add($"mov dword [{pBuffer}], eax");
            m_bufferOffset += 4;

            return this;
        }

        private void moveStateToEax() {
            m_script.Add("mov eax, [esp]");
        }

        private LuaVMWrapper PopNumber() {
            const int popNumber = 0x0084E030;
            moveStateToEax();
            m_script.Add($"push {m_columnState}");
            m_script.Add("push eax");
            m_script.Add(popNumber.CallViaEax());
            m_script.Add("add esp, 8");
            m_columnState++;

            return this;
        }

        private void saveLuaStateOnStack() {
            const int getState = 0x00817DB0;
            m_script.Add(getState.CallViaEax());
            m_script.Add("push eax");
        }

        private LuaVMWrapper SaveNumberTo(int pBuffer) {
            m_script.Add($"mov eax, {pBuffer}");
            m_script.Add("fstp qword [eax]");
            m_bufferOffset += 8;

            return this;
        }

        #endregion Methods
    }
}