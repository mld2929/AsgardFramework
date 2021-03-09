using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal class MODULEENTRY32W
    {
        internal readonly int dwSize;
        internal readonly int th32ModuleID;
        internal readonly int th32ProcessID;
        internal readonly int GlblcntUsage;
        internal readonly int ProccntUsage;
        internal readonly int modBaseAddr;
        internal readonly int modBaseSize;
        internal readonly int hModule;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        internal readonly string szModule;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        internal readonly string szExePath;
    }
}