using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Common
    {
        internal readonly int VFTable; // 4
        private readonly int unknown_0; // 8
        internal readonly int Fields; // 12
        private readonly int unknown_1; // 16
        [MarshalAs(UnmanagedType.U4)]
        public ObjectType Type; // 20
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        private readonly long[] unknown_2; // 60
        internal readonly int Next; // 64
    }
}
