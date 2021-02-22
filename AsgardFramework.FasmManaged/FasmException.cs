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
        #region Constructors

        internal FasmLineErrorException(FasmAssembler.LINE_HEADER data, string message, string? mnemonics) : base(message) {
            ExceptionData = data;

            if (mnemonics == null)
                return;

            Mnemonics = mnemonics;
            BadString = mnemonics.Substring(data.FileOffset);
            BadString = BadString.Substring(0, BadString.IndexOf('\n') + 1);
        }

        #endregion Constructors

        #region Fields

        public readonly string BadString;
        public readonly FasmAssembler.LINE_HEADER ExceptionData;
        public readonly string Mnemonics;

        #endregion Fields
    }
}