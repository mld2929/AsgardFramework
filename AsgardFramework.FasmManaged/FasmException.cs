using System;

namespace AsgardFramework.FasmManaged
{
    public class FasmException : Exception
    {
        #region Constructors

        internal FasmException(string message) : base(message) { }

        #endregion Constructors
    }

    public class FasmLineErrorException : FasmException
    {
        #region Fields

        public readonly FasmAssembler.LINE_HEADER ExceptionData;

        #endregion Fields

        #region Constructors

        internal FasmLineErrorException(FasmAssembler.LINE_HEADER data, string message) : base(message) {
            ExceptionData = data;
        }

        #endregion Constructors
    }
}