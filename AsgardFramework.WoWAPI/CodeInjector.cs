using AsgardFramework.CodeInject;
using AsgardFramework.Memory;
using System.Collections.Generic;
using System.Linq;

namespace AsgardFramework.WoWAPI
{
    internal class CodeInjector : ICodeInject
    {
        private static readonly byte[] c_ret = new byte[] { 0xC5 };
        public void Inject(IMemory memory, ICodeBlock code, int offset) {
            InjectWithoutRet(memory, code, offset);
            memory.Write(offset + code.Compiled.Length, c_ret);
        }
        public void Inject(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset) {
            InjectWithoutRet(memory, blocks, offset);
            memory.Write(blocks.Sum(b => b.Compiled.Length) + offset, c_ret);
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
    }
}
