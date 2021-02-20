using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0xE8)]
    public class Item : Object
    {
        [FieldOffset(-0x18 + 0x20)]
        public readonly ulong Contained;

        [FieldOffset(-0x18 + 0xF8)]
        public readonly uint CreatePlayedTime;

        [FieldOffset(-0x18 + 0x28)]
        public readonly ulong Creator;

        [FieldOffset(-0x18 + 0xF0)]
        public readonly uint Durability;

        [FieldOffset(-0x18 + 0x3C)]
        public readonly uint Duration;

        [FieldOffset(-0x18 + 0x58)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x24)]
        public readonly uint[] Enchantments;

        [FieldOffset(-0x18 + 0x54)]
        public readonly uint Flags;

        [FieldOffset(-0x18 + 0x30)]
        public readonly ulong GiftCreator;

        [FieldOffset(-0x18 + 0xF4)]
        public readonly uint MaxDurability;

        [FieldOffset(-0x18 + 0x18)]
        public readonly ulong Owner;

        [FieldOffset(-0x18 + 0xE8)]
        public readonly uint PropertySeed;

        [FieldOffset(-0x18 + 0xEC)]
        public readonly uint RandomPropertiesID;

        [FieldOffset(-0x18 + 0x40)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5)]
        public readonly uint[] SpellCharges;

        [FieldOffset(0x20)]
        public readonly uint StackCount;
    }
}