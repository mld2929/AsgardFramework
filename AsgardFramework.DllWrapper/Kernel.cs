using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.DllWrapper
{
    internal static class Kernel
    {
        #region Methods

        [DllImport("Kernel32.dll")]
        internal static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("Kernel32.dll")]
        internal static extern IntPtr CreateToolhelp32Snapshot(int dwFlags, int th32ProcessID);

        [DllImport("Kernel32.dll")]
        internal static extern int GetLastError();

        [DllImport("Kernel32.dll", SetLastError = true)]
        internal static extern int GetProcAddress(int hModule, [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("Kernel32.dll")]
        internal static extern bool Module32FirstW(IntPtr hSnapshot, IntPtr lpme);

        [DllImport("Kernel32.dll")]
        internal static extern bool Module32NextW(IntPtr hSnapshot, IntPtr lpme);

        #endregion Methods
    }
}