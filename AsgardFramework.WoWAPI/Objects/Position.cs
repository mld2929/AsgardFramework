using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 16)]
    public class Position
    {
        [FieldOffset(12)]
        public readonly float Rotation;

        [FieldOffset(0)]
        public readonly float X;

        [FieldOffset(4)]
        public readonly float Y;

        [FieldOffset(8)]
        public readonly float Z;
    }
}