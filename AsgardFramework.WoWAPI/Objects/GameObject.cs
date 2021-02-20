using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x30)]
    public sealed class GameObject : Object
    {
        [FieldOffset(-0x18 + 0x44)]
        public readonly uint Bytes_1;

        [FieldOffset(-0x18 + 0x18)]
        public readonly ulong CreatedBy;

        [FieldOffset(-0x18 + 0x20)]
        public readonly uint DisplayID;

        [FieldOffset(-0x18 + 0x38)]
        public readonly uint Dynamic;

        [FieldOffset(-0x18 + 0x3C)]
        public readonly uint Faction;

        [FieldOffset(-0x18 + 0x24)]
        public readonly uint Flags;

        [FieldOffset(-0x18 + 0x40)]
        public readonly uint Level;

        [FieldOffset(-0x18 + 0x28)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public readonly uint[] ParentRotation;
    }
}