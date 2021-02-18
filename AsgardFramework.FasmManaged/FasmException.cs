using System;

namespace AsgardFramework.FasmManaged
{
    public class FasmException : Exception
    {
        internal FasmException(string message) : base(message) {

        }
    }

    public class FasmLineErrorException : FasmException
    {
        public readonly FasmAssembler.LINE_HEADER ExceptionData;
        internal FasmLineErrorException(FasmAssembler.LINE_HEADER data, string message) : base(message) {
            ExceptionData = data;
        }
    }
}
