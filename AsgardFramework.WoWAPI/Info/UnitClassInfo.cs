using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
    public class UnitClassInfo
    {
        /// <summary>
        ///     The localized name of the unit's class, or the unit's name if the unit is an NPC
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string LocalizedName;

        /// <summary>
        ///     A non-localized token representing the class
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Name;
    }
}