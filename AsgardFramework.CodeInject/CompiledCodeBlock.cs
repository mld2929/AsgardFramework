namespace AsgardFramework.CodeInject
{
    public class CompiledCodeBlock : ICodeBlock
    {
        public byte[] Compiled { get; private set; }

        public CompiledCodeBlock(byte[] compiled) {
            Compiled = compiled;
        }
    }
}
