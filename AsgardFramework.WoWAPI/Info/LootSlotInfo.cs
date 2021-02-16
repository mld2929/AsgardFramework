using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LootSlotInfo
    {
        public readonly string Name;
        public readonly string Texture;
        public readonly int Count;
    }
}
