using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Position
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;
        public readonly float Rotation;
    }
}
