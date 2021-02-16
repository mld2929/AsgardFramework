using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using System;
using System.Linq;

namespace AsgardFramework.WoWAPI
{
    internal class LazyCodeBlock : ICodeBlock
    {
        internal LazyCodeBlock(IFasmCompiler compiler, string[] asm) {
            m_compiled = new Lazy<byte[]>(() => compiler.Compile(asm).ToArray());
        }
        private readonly Lazy<byte[]> m_compiled;
        public byte[] Compiled => m_compiled.Value;
    }
}
