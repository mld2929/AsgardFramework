﻿using System;
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
    internal class EndSceneHookExecutor : CriticalHandle, ICodeExecutor
    {
        private const int c_resultOffset = c_flagOffset - 4;
        private const int c_flagOffset = c_executionOffset - 4;
        private const int c_executionOffset = 1024;
        private const int c_hookSpaceSize = 8096;
        private readonly IAutoManagedMemory m_hookSpace;
        private readonly IGlobalMemory m_memory;
        private readonly ICodeInjector m_injector;
        private readonly IFasmAssembler m_compiler;
        private readonly IIDirect3DDevice9Observer m_observer;
        private readonly SemaphoreSlim m_semaphore = new SemaphoreSlim(1, 1);
        private int pResult => m_hookSpace.Start + c_resultOffset;
        private int pFlag => m_hookSpace.Start + c_flagOffset;
        public bool ExecutionFlag { get => m_memory.Read<int>(pFlag) != 0; set => m_memory.Write(pFlag, value ? 1 : 0); }
        public int Result => m_memory.Read<int>(pResult);

        public override bool IsInvalid => m_memory.Read<int>(m_observer.pEndScene) == m_observer.EndScene;

        internal EndSceneHookExecutor(ICodeInjector injector, IGlobalMemory memory, IIDirect3DDevice9Observer observer, IFasmAssembler compiler) : base((IntPtr)observer.EndScene) {
            AppDomain.CurrentDomain.ProcessExit += (_, __) => Dispose();
            m_injector = injector;
            m_memory = memory;
            m_observer = observer;
            m_compiler = compiler;
            m_hookSpace = memory.Allocate(c_hookSpaceSize);
            var hook = new string[]
            {
                "pushad",
                "pushfd",
                $"cmp dword [{m_hookSpace.Start + c_flagOffset}], 0",
                "je @out",
                $"mov eax, {m_hookSpace.Start + c_executionOffset}",
                $"call eax",
                $"mov dword [{m_hookSpace.Start + c_resultOffset}], eax",
                $"mov dword [{m_hookSpace.Start + c_flagOffset}], 0",
                "@out:",
                "popfd",
                "popad",
                $"mov eax, {m_observer.EndScene}",
                $"jmp eax"
            };
            var compiledHook = new CompiledCodeBlock(compiler.Assemble(hook).ToArray());
            m_injector.InjectWithoutRet(m_hookSpace, compiledHook, 0);
            m_memory.Write(m_observer.pEndScene, m_hookSpace.Start);
        }

        public async Task<int> Execute(ICodeBlock code) {
            var injection = code;
            if (code.Compiled.Length > m_hookSpace.Size - c_executionOffset) {
                var newSpace = m_memory.Allocate(code.Compiled.Length);
                m_injector.Inject(newSpace, code, 0);
                injection = JumpToExtraSpace(newSpace);
            }
            await m_semaphore.WaitAsync();
            m_injector.Inject(m_hookSpace, injection, c_executionOffset);
            ExecutionFlag = true;
            while (ExecutionFlag) {
            }
            var result = Result;
            m_semaphore.Release();
            return result;
        }

        private ICodeBlock JumpToExtraSpace(IAutoManagedMemory space) {
            var asm = new string[]
            {
                $"mov eax, {space.Start}",
                "call eax",
                "ret"
            };
            return new CompiledCodeBlock(m_compiler.Assemble(asm).ToArray());
        }
        protected override bool ReleaseHandle() {
            m_memory.Write(m_observer.pEndScene, m_observer.EndScene);
            return IsInvalid;
        }
    }
}
