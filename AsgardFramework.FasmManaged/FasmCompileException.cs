using System;

namespace AsgardFramework.FasmManaged
{
    internal class FasmCompileException : Exception
    {
        public readonly FasmError ErrorCode;
        internal FasmCompileException(FasmError code) {
            ErrorCode = code;
        }
    }
}
