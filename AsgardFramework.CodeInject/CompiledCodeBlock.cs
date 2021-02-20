namespace AsgardFramework.CodeInject
{
    public class CompiledCodeBlock : ICodeBlock
    {
        #region Constructors

        public CompiledCodeBlock(byte[] compiled) {
            Compiled = compiled;
        }

        #endregion Constructors

        #region Properties

        public byte[] Compiled { get; }

        #endregion Properties
    }
}