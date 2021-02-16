using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace AsgardFramework.FasmManaged
{
    public class FasmCompiler : IFasmCompiler
    {
        private const string c_dllName = "FASM.DLL";

        [DllImport(c_dllName, CallingConvention = CallingConvention.StdCall)]
        private static extern FasmError fasm_Assemble([MarshalAs(UnmanagedType.LPStr)] string lpSource,
                                                      IntPtr lpMemory,
                                                      int cbMemorySize,
                                                      ushort nPassesLimit = 100,
                                                      IntPtr hDisplayPipe = default);

        /// <exception cref="FileNotFoundException"/>
        public FasmCompiler() {
            if (!File.Exists(c_dllName)) {
                throw new FileNotFoundException(c_dllName);
            }
        }

        public TargetArchitecture Architecture { get; set; } = TargetArchitecture.x86;

        /// <exception cref="FasmCompileException"/>
        public IEnumerable<byte> Compile(IEnumerable<string> instructions) {
            return Compile(string.Join('\n', instructions));
        }

        /// <exception cref="FasmCompileException"/>
        public IEnumerable<byte> Compile(string instructions) {
            return compile(instructions);
        }

        /// <exception cref="FasmCompileException"/>
        private byte[] compile(string data) {
            var memSize = 1024;
            FasmError code;
            IntPtr pMemory, pMemoryResult;
            data = Architecture.AsString() + data;
            do {
                pMemory = pMemoryResult = Marshal.AllocHGlobal(memSize);
                code = fasm_Assemble(data, pMemoryResult, memSize);
                memSize *= 2;
            } while (code == FasmError.OutOfMemory);

            if (code != FasmError.Success) {
                Marshal.FreeHGlobal(pMemory);
                throw new FasmCompileException(code);
            }
            pMemoryResult += 4;
            var bytesCount = Marshal.ReadInt32(pMemoryResult);
            pMemoryResult += 4;
            var pBytes = Marshal.ReadIntPtr(pMemoryResult);
            var result = new byte[bytesCount];
            Marshal.Copy(pBytes, result, 0, bytesCount);
            Marshal.FreeHGlobal(pMemory);
            return result;
        }
    }
}
