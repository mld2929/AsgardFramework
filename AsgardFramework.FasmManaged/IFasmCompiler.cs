using System.Collections.Generic;

namespace AsgardFramework.FasmManaged
{
    public interface IFasmCompiler
    {
        public TargetArchitecture Architecture { get; set; }

        /// <exception cref="FasmCompileException"/>
        public IEnumerable<byte> Compile(IEnumerable<string> instructions);

        /// <exception cref="FasmCompileException"/>
        public IEnumerable<byte> Compile(string instructions);
    }
}
