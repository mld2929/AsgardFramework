using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1)]
    public class Container : Object
    {
        [FieldOffset(0)]
        public readonly int Slots;

        [field: FieldOffset(8)]
        [field: MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public ulong[] Items { get; internal set; }
    }
}