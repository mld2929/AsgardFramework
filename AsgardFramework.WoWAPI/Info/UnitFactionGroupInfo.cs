using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
    public class UnitFactionGroupInfo
    {
        /// <summary>
        ///     Localized name of the faction
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string LocalizedName;

        /// <summary>
        ///     Non-localized (English) faction name of the faction (<c>"Horde"</c> or <c>"Alliance"</c>)
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Name;
    }
}