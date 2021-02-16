using AsgardFramework.Memory;
using System.Collections.Generic;

namespace AsgardFramework.CodeInject
{
    public interface ICodeInject
    {
        void Inject(IMemory memory, ICodeBlock code, int offset);
        void InjectWithoutRet(IMemory memory, ICodeBlock code, int offset);
        void Inject(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset);
        void InjectWithoutRet(IMemory memory, IEnumerable<ICodeBlock> blocks, int offset);
    }
}
