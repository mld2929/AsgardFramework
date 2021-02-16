using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential)]
    public class SpellInfo
    {
        public readonly int ID;
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringMarshaler))]
        public readonly string Name;
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringMarshaler))]
        public readonly string Rank;
        [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(StringMarshaler))]
        public readonly string Icon;
        public readonly int Cost;
        public readonly bool isFunnel;
        public readonly int PowerType;
        public readonly int CastTime;
        public readonly int MinRange;
        public readonly int MaxRange;
    }
}
