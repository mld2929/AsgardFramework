using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 64)]
    public class Common
    {
        [FieldOffset(0x8)]
        internal readonly int Fields;

        [FieldOffset(0x30)]
        public readonly ulong Guid;

        [FieldOffset(0x3C)]
        internal readonly int Next;

        [FieldOffset(0x14)]
        [MarshalAs(UnmanagedType.U4)]
        public readonly ObjectType Type;

        [FieldOffset(0)]
        internal readonly int VFTable;
    }
}