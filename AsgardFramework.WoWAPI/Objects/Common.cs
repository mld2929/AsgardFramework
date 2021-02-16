using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Sequential)]
    public class Common
    {
        public readonly int Base;
        public readonly int Fields;
        private readonly long unknown;
        [MarshalAs(UnmanagedType.U4)]
        public ObjectType Type;
    }
}
