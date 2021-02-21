using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using AsgardFramework.CodeInject;
using AsgardFramework.DirectXObserver;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal sealed class EndSceneHookExecutor : SafeHandle, ICodeExecutor
    {
        #region Constructors

        internal EndSceneHookExecutor(ICodeInjector injector, IGlobalMemory memory, IIDirect3DDevice9Observer observer, IFasmAssembler compiler) : base(IntPtr.Zero, true) {
            handle = (IntPtr)observer.EndScene;

            m_injector = injector;
            m_memory = memory;
            m_observer = observer;
            m_compiler = compiler;
            m_hookSpace = memory.Allocate(c_hookSpaceSize);

            var hook = new[] {
                "pushad",
                "pushfd",
                $"cmp dword [{m_hookSpace + c_flagOffset}], 0",
                "je @out",
                (m_hookSpace.Start + c_executionOffset).CallViaEax(),
                $"mov dword [{m_hookSpace + c_resultOffset}], eax",
                $"mov dword [{m_hookSpace + c_flagOffset}], 0",
                "@out:",
                "popfd",
                "popad",
                $"mov eax, {m_observer.EndScene}",
                "jmp eax"
            };

            var compiledHook = new CompiledCodeBlock(compiler.Assemble(hook)
                                                             .ToArray());

            m_injector.InjectWithoutRet(m_hookSpace, compiledHook, 0);
            m_memory.Write(m_observer.pEndScene, m_hookSpace.Start);
            var weakThis = new WeakReference<EndSceneHookExecutor>(this);

            m_hookSpace.Disposing += (_, __) => {
                if (weakThis.TryGetTarget(out var @this))
                    @this.Dispose();
            };
        }

        #endregion Constructors

        #region Fields

        private const int c_executionOffset = 1024;
        private const int c_flagOffset = c_executionOffset - 4;
        private const int c_hookSpaceSize = 8096;
        private const int c_resultOffset = c_flagOffset - 4;
        private readonly IFasmAssembler m_compiler;
        private readonly IAutoManagedMemory m_hookSpace;
        private readonly ICodeInjector m_injector;
        private readonly IGlobalMemory m_memory;
        private readonly IIDirect3DDevice9Observer m_observer;
        private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1, 1);

        #endregion Fields

        #region Properties

        public bool ExecutionFlag {
            get => m_memory.Read<int>(m_pFlag) != 0;
            set => m_memory.Write(m_pFlag, value ? 1 : 0);
        }

        public override bool IsInvalid => m_memory.Read<int>(m_observer.pEndScene) == m_observer.EndScene;
        public int Result => m_memory.Read<int>(m_pResult);
        private int m_pFlag => m_hookSpace + c_flagOffset;
        private int m_pResult => m_hookSpace + c_resultOffset;

        #endregion Properties

        #region Methods

        public async Task<int> ExecuteAsync(ICodeBlock code) {
            if (IsClosed)
                throw new ObjectDisposedException(nameof(EndSceneHookExecutor));

            var injection = code;

            if (code.Compiled.Length > m_hookSpace.Size - c_executionOffset) {
                var newSpace = m_memory.Allocate(code.Compiled.Length);
                m_injector.Inject(newSpace, code, 0);
                injection = jumpToExtraSpace(newSpace);
            }

            await m_semaphore.WaitAsync()
                             .ConfigureAwait(false);

            m_injector.Inject(m_hookSpace, injection, c_executionOffset);
            ExecutionFlag = true;

            while (ExecutionFlag)
                await Task.Delay(1)
                          .ConfigureAwait(false);

            var result = Result;
            m_semaphore.Release();

            return result;
        }

        protected override bool ReleaseHandle() {
            m_memory.Write(m_observer.pEndScene, m_observer.EndScene);

            return IsClosed;
        }

        private ICodeBlock jumpToExtraSpace(IAutoManagedMemory space) {
            var asm = new[] {
                space.Start.CallViaEax(),
                "ret"
            };

            return new CompiledCodeBlock(m_compiler.Assemble(asm)
                                                   .ToArray());
        }

        #endregion Methods
    }
}