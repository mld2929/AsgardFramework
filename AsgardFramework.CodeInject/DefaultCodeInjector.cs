using System.Collections.Generic;
using System.Linq;

using AsgardFramework.Memory;

namespace AsgardFramework.CodeInject
{
    public class DefaultCodeInjector : ICodeInjector
    {
        #region Fields

        private static readonly byte[] Ret = {
            0xC3
        };

        #endregion Fields

        #region Methods

        public void Inject(IMemory memory, ICodeBlock code, int offset) {
            InjectWithoutRet(memory, code, offset);
            memory.Write(offset + code.Compiled.Length, Ret);
        }

        public void Inject(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset) {
            var codeBlocks = blocks.ToList();
            InjectWithoutRet(memory, codeBlocks, offset);
            memory.Write(codeBlocks.Sum(b => b.Compiled.Length) + offset, Ret);
        }

        public void InjectWithoutRet(IMemory memory, ICodeBlock code, int offset) {
            memory.Write(offset, code.Compiled);
        }

        public void InjectWithoutRet(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset) {
            foreach (var block in blocks) {
                memory.Write(offset, block.Compiled);
                offset += block.Compiled.Length;
            }
        }

        #endregion Methods
    }
}