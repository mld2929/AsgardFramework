using AsgardFramework.CodeInject;
using AsgardFramework.DirectXObserver;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;
using System.Linq;
using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    internal class EndSceneHookExecutor : ICodeExecutor
    {
        private const int c_resultOffset = c_flagOffset - 4;
        private const int c_flagOffset = c_executionOffset - 4;
        private const int c_executionOffset = 1024;
        private const int c_hookSpaceSize = 8096;
        private readonly IAutoManagedMemory m_hookSpace;
        private readonly IGlobalMemory m_memory;
        private readonly ICodeInject m_injector;
        private readonly IFasmCompiler m_compiler;
        private readonly IDirect3DDevice9Observer m_observer;
        private int pResult => m_hookSpace.Start + c_resultOffset;
        private int pFlag => m_hookSpace.Start + c_flagOffset;
        public bool ExecutionFlag { get => m_memory.Read(pFlag, 4).ToInt32() != 0; set => m_memory.Write(pFlag, value.ToBytes()); }
        public int Result => m_memory.Read(pResult, 4).ToInt32();

        internal EndSceneHookExecutor(ICodeInject injector, IGlobalMemory memory, IDirect3DDevice9Observer observer, IFasmCompiler compiler)
        {
            m_injector = injector;
            m_memory = memory;
            m_observer = observer;
            m_compiler = compiler;

            // todo: write hook
            var hook = new string[]
            {
                "pushad",
                "pushfd",
                "popfd",
                "popad",
                $"mov eax, {m_observer.EndScene}",
                "jmp eax"
            };

            var compiledHook = new CompiledCodeBlock(compiler.Compile(hook).ToArray());

            m_hookSpace = memory.Allocate(c_hookSpaceSize);
            m_injector.InjectWithoutRet(m_hookSpace, compiledHook, 0);
            m_memory.Write(m_observer.pEndScene, m_hookSpace.Start.ToBytes());
        }
        ~EndSceneHookExecutor()
        {
            m_memory.Write(m_observer.pEndScene, m_observer.EndScene.ToBytes());
        }

        // todo: thread-safe
        public Task<int> Execute(ICodeBlock code)
        {
            var injection = code;
            if (code.Compiled.Length > m_hookSpace.Size - c_executionOffset)
            {
                var newSpace = m_memory.Allocate(code.Compiled.Length);
                m_injector.Inject(newSpace, code, 0);
                injection = JumpToExtraSpace(newSpace);
            }
            m_injector.Inject(m_hookSpace, injection, c_executionOffset);
            ExecutionFlag = true;
            return Task.Run(() =>
            {
                while (ExecutionFlag) ;

                return Result;
            });
        }

        private ICodeBlock JumpToExtraSpace(IAutoManagedMemory space)
        {
            var asm = new string[]
            {
                $"mov eax, {space.Start}",
                "call eax"
            };
            return new CompiledCodeBlock(m_compiler.Compile(asm).ToArray());
        }
    }
}
