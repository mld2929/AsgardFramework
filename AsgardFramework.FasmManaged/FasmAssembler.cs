using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace AsgardFramework.FasmManaged
{
    public class FasmAssembler : IFasmAssembler
    {
        private const string c_dllName = "FASM.DLL";
        private const int c_defaultBufferSize = 8 * 1024 * 1024; // 8 MB
        private readonly byte[] m_buffer = new byte[c_defaultBufferSize];
        private readonly object locker = new object();

        public string FasmDLLVersion {
            get {
                uint version = fasm_GetVersion();
                return unchecked($"{((ushort)(version)).ToString(CultureInfo.InvariantCulture)}.{((ushort)(version >> 16)).ToString(CultureInfo.InvariantCulture)}");
            }
        }
        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 12)]
        private class FASM_STATE
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.U4)]
            public readonly FASM_CONDITION Condition;
            [FieldOffset(4)]
            public readonly int OutputLength;
            [FieldOffset(4)]
            [MarshalAs(UnmanagedType.U4)]
            public readonly FASMERR ErrorCode;
            [FieldOffset(8)]
            public readonly unsafe void* OutputData;
            [FieldOffset(8)]
            public readonly unsafe void* ErrorLine;
        }

        [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
        public class LINE_HEADER
        {
            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.LPStr)]
            public readonly string FilePath;
            [FieldOffset(4)]
            public readonly int LineNumber;
            [FieldOffset(8)]
            public readonly int FileOffset;
            [FieldOffset(8)]
            public readonly unsafe void* MacroCallingLine;
            [FieldOffset(12)]
            public readonly unsafe void* MacroLine;
        }

        private enum FASM_CONDITION
        {
            FASM_OK = 0, // FASM_STATE points to output
            FASM_WORKING = 1,
            FASM_ERROR = 2, // FASM_STATE contains error code
            FASM_INVALID_PARAMETER = -1,
            FASM_OUT_OF_MEMORY = -2,
            FASM_STACK_OVERFLOW = -3,
            FASM_SOURCE_NOT_FOUND = -4,
            FASM_UNEXPECTED_END_OF_SOURCE = -5,
            FASM_CANNOT_GENERATE_CODE = -6,
            FASM_FORMAT_LIMITATIONS_EXCEDDED = -7,
            FASM_WRITE_FAILED = -8,
            FASM_INVALID_DEFINITION = -9
        }

        private enum FASMERR
        {
            FASMERR_FILE_NOT_FOUND = -101,
            FASMERR_ERROR_READING_FILE = -102,
            FASMERR_INVALID_FILE_FORMAT = -103,
            FASMERR_INVALID_MACRO_ARGUMENTS = -104,
            FASMERR_INCOMPLETE_MACRO = -105,
            FASMERR_UNEXPECTED_CHARACTERS = -106,
            FASMERR_INVALID_ARGUMENT = -107,
            FASMERR_ILLEGAL_INSTRUCTION = -108,
            FASMERR_INVALID_OPERAND = -109,
            FASMERR_INVALID_OPERAND_SIZE = -110,
            FASMERR_OPERAND_SIZE_NOT_SPECIFIED = -111,
            FASMERR_OPERAND_SIZES_DO_NOT_MATCH = -112,
            FASMERR_INVALID_ADDRESS_SIZE = -113,
            FASMERR_ADDRESS_SIZES_DO_NOT_AGREE = -114,
            FASMERR_DISALLOWED_COMBINATION_OF_REGISTERS = -115,
            FASMERR_LONG_IMMEDIATE_NOT_ENCODABLE = -116,
            FASMERR_RELATIVE_JUMP_OUT_OF_RANGE = -117,
            FASMERR_INVALID_EXPRESSION = -118,
            FASMERR_INVALID_ADDRESS = -119,
            FASMERR_INVALID_VALUE = -120,
            FASMERR_VALUE_OUT_OF_RANGE = -121,
            FASMERR_UNDEFINED_SYMBOL = -122,
            FASMERR_INVALID_USE_OF_SYMBOL = -123,
            FASMERR_NAME_TOO_LONG = -124,
            FASMERR_INVALID_NAME = -125,
            FASMERR_RESERVED_WORD_USED_AS_SYMBOL = -126,
            FASMERR_SYMBOL_ALREADY_DEFINED = -127,
            FASMERR_MISSING_END_QUOTE = -128,
            FASMERR_MISSING_END_DIRECTIVE = -129,
            FASMERR_UNEXPECTED_INSTRUCTION = -130,
            FASMERR_EXTRA_CHARACTERS_ON_LINE = -131,
            FASMERR_SECTION_NOT_ALIGNED_ENOUGH = -132,
            FASMERR_SETTING_ALREADY_SPECIFIED = -133,
            FASMERR_DATA_ALREADY_DEFINED = -134,
            FASMERR_TOO_MANY_REPEATS = -135,
            FASMERR_SYMBOL_OUT_OF_SCOPE = -136,
            FASMERR_USER_ERROR = -140,
            FASMERR_ASSERTION_FAILED = -141
        }

        [AllowReversePInvokeCalls]
        [DllImport(c_dllName, CallingConvention = CallingConvention.StdCall)]
        private static extern FASM_CONDITION fasm_Assemble([MarshalAs(UnmanagedType.LPStr)] string lpSource,
                                                      [MarshalAs(UnmanagedType.LPArray)] byte[] lpMemory,
                                                      int cbMemorySize,
                                                      ushort nPassesLimit = ushort.MaxValue,
                                                      IntPtr hDisplayPipe = default);

        [AllowReversePInvokeCalls]
        [SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
        [DllImport(c_dllName, CallingConvention = CallingConvention.StdCall)]
        private static extern FASM_CONDITION fasm_AssembleFile([MarshalAs(UnmanagedType.LPWStr)] string lpSourceFile,
                                                      [MarshalAs(UnmanagedType.LPArray)] byte[] lpMemory,
                                                      int cbMemorySize,
                                                      ushort nPassesLimit = ushort.MaxValue,
                                                      IntPtr hDisplayPipe = default);
        [DllImport(c_dllName, CallingConvention = CallingConvention.StdCall)]
        private static extern uint fasm_GetVersion();

        /// <exception cref="FileNotFoundException"/>
        public FasmAssembler() {
            if (!File.Exists(c_dllName)) {
                throw new FileNotFoundException(c_dllName);
            }
        }
        private unsafe byte[] assemble(string data, TargetArchitecture architecture, bool fromFile, TextWriter output) {
            data = fromFile ? data : architecture.AsString() + data;
            GCHandle? gchandle = output == null ? (GCHandle?)null : GCHandle.Alloc(output);
            IntPtr handle = gchandle.HasValue ? GCHandle.ToIntPtr(gchandle.Value) : IntPtr.Zero;
            lock (locker) {
                Array.Fill<byte>(m_buffer, 0);
                byte[] buffer = m_buffer;
                FASM_CONDITION condition = fromFile ? fasm_AssembleFile(data, buffer, c_defaultBufferSize, hDisplayPipe: handle) : fasm_Assemble(data, buffer, c_defaultBufferSize, hDisplayPipe: handle);
                if (condition == FASM_CONDITION.FASM_OUT_OF_MEMORY) {
                    int extraMemorySize = c_defaultBufferSize;
                    do {
                        extraMemorySize += c_defaultBufferSize;
                        buffer = new byte[extraMemorySize];
                        condition = fromFile ? fasm_AssembleFile(data, buffer, extraMemorySize, hDisplayPipe: handle) : fasm_Assemble(data, buffer, extraMemorySize, hDisplayPipe: handle);
                    } while (condition == FASM_CONDITION.FASM_OUT_OF_MEMORY);
                }
                gchandle?.Free();
                fixed (byte* bytes = buffer) {
                    FASM_STATE state = Marshal.PtrToStructure<FASM_STATE>((IntPtr)bytes);
                    switch (condition) {
                        case FASM_CONDITION.FASM_OK:
                            byte[] result = new byte[state.OutputLength];
                            Marshal.Copy((IntPtr)state.OutputData, result, 0, state.OutputLength);
                            return result;
                        case FASM_CONDITION.FASM_ERROR:
                            throw new FasmLineErrorException(Marshal.PtrToStructure<LINE_HEADER>((IntPtr)state.ErrorLine), state.ErrorCode.ToString());
                        case FASM_CONDITION.FASM_WORKING:
                        case FASM_CONDITION.FASM_INVALID_PARAMETER:
                        case FASM_CONDITION.FASM_OUT_OF_MEMORY:
                        case FASM_CONDITION.FASM_STACK_OVERFLOW:
                        case FASM_CONDITION.FASM_SOURCE_NOT_FOUND:
                        case FASM_CONDITION.FASM_UNEXPECTED_END_OF_SOURCE:
                        case FASM_CONDITION.FASM_CANNOT_GENERATE_CODE:
                        case FASM_CONDITION.FASM_FORMAT_LIMITATIONS_EXCEDDED:
                        case FASM_CONDITION.FASM_WRITE_FAILED:
                        case FASM_CONDITION.FASM_INVALID_DEFINITION:
                            throw new FasmException(condition.ToString());
                        default:
                            throw new Exception("Unknown error");
                    }
                }
            }
        }

        public IEnumerable<byte> Assemble(IEnumerable<string> instructions, TargetArchitecture architecture = TargetArchitecture.x86, TextWriter output = null) {
            return assemble(string.Join('\n', instructions), architecture, false, output);
        }

        public IEnumerable<byte> Assemble(string instructions, TargetArchitecture architecture = TargetArchitecture.x86, TextWriter output = null) {
            return assemble(instructions, architecture, false, output);
        }

        public IEnumerable<byte> AssembleFile(string filePath, TextWriter output = null) {
            return assemble(filePath, default, true, output);
        }
    }
}
