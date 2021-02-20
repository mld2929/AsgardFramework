using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 8)]
    public class ItemSpellInfo
    {
        /// <summary>
        ///     Name of the spell
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Name;

        /// <summary>
        ///     Secondary text associated with the spell
        /// </summary>
        /// <remarks>
        ///     Often a rank, e.g. <c>"Rank 7"</c>; or the <see cref="string.Empty" /> if not applicable
        /// </remarks>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string RankOrSecondaryText;
    }
}