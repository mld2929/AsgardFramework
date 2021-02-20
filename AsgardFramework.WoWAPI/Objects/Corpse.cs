using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x74)]
    public sealed class Corpse : Object
    {
        [FieldOffset(-0x18 + 0x74)]
        public readonly uint Bytes_1;

        [FieldOffset(-0x18 + 0x78)]
        public readonly uint Bytes_2;

        [FieldOffset(-0x18 + 0x28)]
        public readonly uint DisplayID;

        [FieldOffset(-0x18 + 0x84)]
        public readonly uint DynamicFlags;

        [FieldOffset(-0x18 + 0x80)]
        public readonly uint Flags;

        [FieldOffset(-0x18 + 0x7C)]
        public readonly uint Guild;

        [FieldOffset(-0x18 + 0x2C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x9)]
        public readonly ulong[] Items;

        [FieldOffset(-0x18 + 0x18)]
        public readonly ulong Owner;

        [FieldOffset(-0x18 + 0x20)]
        public readonly ulong Party;
    }
}