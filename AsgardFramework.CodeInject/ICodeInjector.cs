using System.Collections.Generic;

using AsgardFramework.Memory;

namespace AsgardFramework.CodeInject
{
    public interface ICodeInjector
    {
        void Inject(IMemory memory, ICodeBlock code, int offset);
        void InjectWithoutRet(IMemory memory, ICodeBlock code, int offset);
        void Inject(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset);
        void InjectWithoutRet(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset);
    }
}
