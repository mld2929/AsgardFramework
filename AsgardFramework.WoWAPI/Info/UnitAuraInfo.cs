using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 44)]
    public class UnitAuraInfo
    {
        /// <summary>
        ///     Unit which applied the aura. <see cref="string.Empty" /> if the casting unit (or its controller) has no unitID
        /// </summary>
        /// <remarks>
        ///     If the aura was applied by a unit that does not have a token but is controlled by one that does (e.g. a totem or
        ///     another player's vehicle), value is the controlling unit meta id.
        /// </remarks>
        [FieldOffset(28)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string CasterUnitMetaId;

        /// <summary>
        ///     <see langword="true" /> if the aura is eligible for the 'consolidated' aura display in the default UI.
        /// </summary>
        [FieldOffset(36)]
        [MarshalAs(UnmanagedType.U4)]
        public readonly bool ConsolidatedAtUI;

        /// <summary>
        ///     The number of times the aura has been applied
        /// </summary>
        [FieldOffset(12)]
        public readonly int Count;

        /// <summary>
        ///     Total duration of the aura (in seconds)
        /// </summary>
        [FieldOffset(20)]
        public readonly int Duration;

        /// <summary>
        ///     Time at which the aura will expire
        /// </summary>
        [FieldOffset(24)]
        public readonly float ExpiresAt;

        /// <summary>
        ///     Path to an icon texture for the aura
        /// </summary>
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Icon;

        /// <summary>
        ///     Name of the aura
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Name;

        /// <summary>
        ///     Secondary text for the aura (often a rank; e.g. <c>"Rank 7"</c>)
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string RankOrSecondaryText;

        /// <summary>
        ///     Spell ID of the aura
        /// </summary>
        [FieldOffset(40)]
        public readonly int SpellId;

        /// <summary>
        ///     <see langword="true" /> if the aura can be transferred to a player using the Spellsteal spell; otherwise
        ///     <see langword="false" />
        /// </summary>
        [FieldOffset(32)]
        [MarshalAs(UnmanagedType.U4)]
        public readonly bool Stealable;

        /// <summary>
        ///     Type of aura (relevant for dispelling and certain other mechanics); <see cref="string.Empty" /> if not one of the
        ///     following values:
        ///     <list type="table">
        ///         <item>
        ///             <b>Curse</b>
        ///         </item>
        ///         <item>
        ///             <b>Disease</b>
        ///         </item>
        ///         <item>
        ///             <b>Magic</b>
        ///         </item>
        ///         <item>
        ///             <b>Poison</b>
        ///         </item>
        ///     </list>
        /// </summary>
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Type;
    }
}