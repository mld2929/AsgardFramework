using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x10)]
    public sealed class Container : Item
    {
        [FieldOffset(-0xe8 - 0x18 + 0x108)]
        public readonly ulong FirstSlotGuid;

        [FieldOffset(-0xe8 - 0x18 + 0x100)]
        public readonly uint Slots;
    }
}