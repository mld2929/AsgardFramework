using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SpellInfo
    {
        public readonly int ID;
        public readonly string Name;
        public readonly string Rank;
        public readonly string Icon;
        public readonly int Cost;
        public readonly bool isFunnel;
        public readonly int PowerType;
        public readonly int CastTime;
        public readonly int MinRange;
        public readonly int MaxRange;
    }
}
