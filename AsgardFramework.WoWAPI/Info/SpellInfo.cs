using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 36)]
    public class SpellInfo
    {
        /// <summary>
        ///     Casting time of the spell in milliseconds
        /// </summary>
        [FieldOffset(24)]
        public readonly int CastTime;

        /// <summary>
        ///     Amount of mana, rage, energy, runic power, or focus required to cast the spell
        /// </summary>
        [FieldOffset(12)]
        public readonly int Cost;

        /// <summary>
        ///     Path to an icon texture for the spel
        /// </summary>
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Icon;

        /// <summary>
        ///     <see langword="true" /> for spells with health funneling effects (like Health Funnel)
        /// </summary>
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.U4)]
        public readonly bool IsFunnel;

        /// <summary>
        ///     Maximum range from the target at which you can cast the spell
        /// </summary>
        [FieldOffset(32)]
        public readonly int MaxRange;

        /// <summary>
        ///     Minimum range from the target required to cast the spell
        /// </summary>
        [FieldOffset(28)]
        public readonly int MinRange;

        /// <summary>
        ///     Name of the spell
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Name;

        /// <summary>
        ///     Power type to cast the spell
        /// </summary>
        /// <remarks>
        ///     <list type="table">
        ///         <item>
        ///             <term>-2</term>
        ///             <description>Health</description>
        ///         </item>
        ///         <item>
        ///             <term>0</term>
        ///             <description>Mana</description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>Rage</description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>Focus</description>
        ///         </item>
        ///         <item>
        ///             <term>3</term>
        ///             <description>Energy</description>
        ///         </item>
        ///         <item>
        ///             <term>5</term>
        ///             <description>Runes</description>
        ///         </item>
        ///         <item>
        ///             <term>6</term>
        ///             <description>Runic Power</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        [FieldOffset(20)]
        public readonly int PowerType;

        /// <summary>
        ///     Secondary text associated with the spell (e.g.<c>"Rank 5"</c>, <c>"Racial"</c>, etc.)
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string RankOrSecondaryText;
    }
}