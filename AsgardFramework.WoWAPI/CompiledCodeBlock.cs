using AsgardFramework.CodeInject;

namespace AsgardFramework.WoWAPI
{
    internal class CompiledCodeBlock : ICodeBlock
    {
        public byte[] Compiled { get; private set; }

        internal CompiledCodeBlock(byte[] compiled) {
            Compiled = compiled;
        }
    }
}
