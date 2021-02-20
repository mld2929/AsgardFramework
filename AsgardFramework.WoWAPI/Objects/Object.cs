using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x18)]
    public class Object
    {
        [FieldOffset(0xC)]
        public readonly uint EntryID;

        [FieldOffset(0x0)]
        public readonly ulong Guid;

        [FieldOffset(0x8)]
        public readonly int ObjectType;

        [FieldOffset(0x10)]
        public readonly float Scale;
    }
}