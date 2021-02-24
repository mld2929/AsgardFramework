using System.Runtime.InteropServices;

namespace AsgardFramework.DllWrapper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct MODULEENTRY32W
    {
        internal int dwSize;
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