using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x18)]
    public sealed class DynamicObject : Object
    {
        [FieldOffset(-0x18 + 0x20)]
        public readonly uint Bytes;

        [FieldOffset(-0x18 + 0x18)]
        public readonly ulong Caster;

        [FieldOffset(-0x18 + 0x2C)]
        public readonly uint CastTime;

        [FieldOffset(-0x18 + 0x28)]
        public readonly uint Radius;

        [FieldOffset(-0x18 + 0x24)]
        public readonly uint SpellID;
    }
}