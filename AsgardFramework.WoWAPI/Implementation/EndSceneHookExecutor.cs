using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using AsgardFramework.CodeInject;
using AsgardFramework.DirectXObserver;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal sealed class EndSceneHookExecutor : SafeHandle, ICodeExecutor
    {
        #region Constructors

        internal EndSceneHookExecutor(ICodeInjector injector, IGlobalMemory memory, IIDirect3DDevice9Observer observer, IFasmAssembler assembler) : base(IntPtr.Zero, true) {
            handle = (IntPtr)observer.EndScene;
            m_injector = injector;
            m_memory = memory;
            m_observer = observer;
            m_assembler = assembler;
            m_hookSpace = memory.Allocate(c_hookSpaceSize);
            m_permanentActions = new PermanentActions(memory.AllocateAutoScalingShared(c_hookSpaceSize), m_injector, m_assembler);

            var hook = new[] {
                "pushad",
                "pushfd",
                $"cmp dword [{m_hookSpace + c_flagOffset}], 0",
                "je @toPermanent",
                (m_hookSpace.Start + c_executionOffset).CallViaEax(),
                $"mov dword [{m_hookSpace + c_resultOffset}], eax",
                $"mov dword [{m_hookSpace + c_flagOffset}], 0",
                "@toPermanent:",
                $"cmp dword [{m_hookSpace + c_permanentActionsLockNotNeeded}], 0",
                "je @lockSet",
                $"mov dword [{m_hookSpace + c_permanentActionsLockSet}], 0",
                m_permanentActions.ActionsStartAddress.CallViaEax(),
                "jmp @toAntiAfk",
                "@lockSet:",
                $"mov dword [{m_hookSpace + c_permanentActionsLockSet}], 1",
                "@toAntiAfk:",
                $"cmp dword [{m_hookSpace + c_antiAfkEnabled}], 0",
                "je @exit",
                $"mov eax, [{c_timestamp}]",
                $"mov dword [{c_lastHardwareAction}], eax",
                "@exit:",
                "popfd",
                "popad",
                $"mov eax, {m_observer.EndScene}",
                "jmp eax"
            };

            var compiledHook = assembler.Assemble(hook)
                                        .ToCodeBlock();

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

        private const int c_antiAfkEnabled = c_permanentActionsLockSet - 4;
        private const int c_executionOffset = 1024;
        private const int c_flagOffset = c_executionOffset - 4;
        private const int c_hookSpaceSize = 8096;
        private const int c_lastHardwareAction = 0x00B499A4;
        private const int c_permanentActionsLockNotNeeded = c_resultOffset - 4;
        private const int c_permanentActionsLockSet = c_permanentActionsLockNotNeeded - 4;
        private const int c_resultOffset = c_flagOffset - 4;
        private const int c_timestamp = 0x00B1D618;
        private readonly IFasmAssembler m_assembler;

        private readonly IAutoManagedMemory m_hookSpace;

        private readonly ICodeInjector m_injector;

        private readonly IGlobalMemory m_memory;

        private readonly IIDirect3DDevice9Observer m_observer;

        private readonly PermanentActions m_permanentActions;

        private readonly SemaphoreSlim m_permanentActionsSemaphore = new SemaphoreSlim(1, 1);

        private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1, 1);

        #endregion Fields

        #region Properties

        public bool AntiAFK {
            set => m_memory.Write(m_hookSpace + c_antiAfkEnabled, value ? 1 : 0);
        }

        public override bool IsInvalid => m_memory.Read<int>(m_observer.pEndScene) == m_observer.EndScene;

        public int Result => m_memory.Read<int>(m_pResult);

        private bool m_executionFlag {
            get => m_memory.Read<int>(m_pFlag) != 0;
            set => m_memory.Write(m_pFlag, value ? 1 : 0);
        }

        private bool m_permanentActionsLocked {
            set => m_memory.Write(m_hookSpace + c_permanentActionsLockNotNeeded, value ? 0 : 1);
            get => m_memory.Read<int>(m_hookSpace + c_permanentActionsLockSet) != 0;
        }

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
            m_executionFlag = true;

            while (m_executionFlag)
                await Task.Delay(1)
                          .ConfigureAwait(false);

            var result = Result;
            m_semaphore.Release();

            return result;
        }

        public async Task StartExecutePermanentlyAsync(ICodeBlock code) {
            await m_permanentActionsSemaphore.WaitAsync()
                                             .ConfigureAwait(false);

            m_permanentActionsLocked = true;

            while (!m_permanentActionsLocked)
                await Task.Delay(1)
                          .ConfigureAwait(false);

            m_permanentActions.Add(code);
            m_permanentActionsLocked = false;
            m_permanentActionsSemaphore.Release();
        }

        protected override bool ReleaseHandle() {
            m_memory.Write(m_observer.pEndScene, m_observer.EndScene);

            return IsClosed;
        }

        private ICodeBlock jumpToExtraSpace(IAutoManagedMemory space) {
            return m_assembler.Assemble(space.Start.CallViaEax())
                              .ToCodeBlock();
        }

        #endregion Methods
    }
}