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

        internal FasmLineErrorException(FasmAssembler.LINE_HEADER data, string message, string mnemonics) : base(message) {
            ExceptionData = data;

            if (mnemonics == null)
                return;

            Mnemonics = mnemonics;

            if (data?.FileOffset >= mnemonics.Length)
                return;

            BadString = mnemonics.Substring(data.FileOffset);
            var l = BadString.IndexOf('\n') + 1;

            if (l < BadString.Length)
                BadString = BadString.Substring(0, l);
        }

        #endregion Constructors

        #region Fields

        public readonly string BadString;
        public readonly FasmAssembler.LINE_HEADER ExceptionData;
        public readonly string Mnemonics;

        #endregion Fields
    }
}