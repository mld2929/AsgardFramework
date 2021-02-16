using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    internal static class Kernel
    {
        // Vista+
        internal const uint c_allAccess = 0x1FFFFF;
        internal const uint c_memRelease = 0x00008000;
        internal const uint c_memCommitAndReserve = 0x3000;
        internal const uint c_pageReadWriteExecute = 0x40;
        [DllImport("Kernel32.dll")]
        internal static extern SafeHandle OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("Kernel32.dll")]
        internal static extern bool VirtualFreeEx(SafeHandle hProcess, IntPtr lpAddress, uint dwSize = 0, uint dwFreeType = c_memRelease);

        [DllImport("Kernel32.dll")]
        internal static extern IntPtr VirtualAllocEx(SafeHandle hProcess, IntPtr lpAddress, int dwSize, uint flAllocationType = c_memCommitAndReserve, uint flProtect = c_pageReadWriteExecute);

        [DllImport("Kernel32.dll")]
        internal static extern bool ReadProcessMemory(SafeHandle hProcess, int lpBaseAddress, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("Kernel32.dll")]
        internal static extern bool WriteProcessMemory(SafeHandle hProcess, int lpBaseAddress, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);
        [DllImport("Kernel32.dll")]
        internal static extern bool ReadProcessMemory(SafeHandle hProcess, IntPtr lpBaseAddress, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("Kernel32.dll")]
        internal static extern bool WriteProcessMemory(SafeHandle hProcess, IntPtr lpBaseAddress, [MarshalAs(UnmanagedType.LPArray)] byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);
        [DllImport("Kernel32.dll")]
        internal static extern bool ReadProcessMemory(SafeHandle hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("Kernel32.dll")]
        internal static extern bool WriteProcessMemory(SafeHandle hProcess, int lpBaseAddress, IntPtr lpBuffer, int nSize, out int lpNumberOfBytesWritten);

        [DllImport("Kernel32.dll")]
        internal static extern bool ReadProcessMemory(SafeHandle hProcess, int lpBaseAddress, [MarshalAs(UnmanagedType.LPStruct)] object lpBuffer, int nSize, out int lpNumberOfBytesRead);

        [DllImport("Kernel32.dll")]
        internal static extern bool WriteProcessMemory(SafeHandle hProcess, int lpBaseAddress, [MarshalAs(UnmanagedType.LPStruct)] object lpBuffer, int nSize, out int lpNumberOfBytesWritten);
    }
}
