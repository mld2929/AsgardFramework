using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SpellInfo
    {
        /// <summary>
        /// Name of the spell
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Secondary text associated with the spell (e.g.<c>"Rank 5"</c>, <c>"Racial"</c>, etc.)
        /// </summary>
        public readonly string RankOrSecondaryText;
        /// <summary>
        /// Path to an icon texture for the spel
        /// </summary>
        public readonly string Icon;
        /// <summary>
        /// Amount of mana, rage, energy, runic power, or focus required to cast the spell
        /// </summary>
        public readonly int Cost;
        /// <summary>
        /// <see langword="true"/> for spells with health funneling effects (like Health Funnel)
        /// </summary>
        public readonly bool IsFunnel;
        /// <summary>
        /// Power type to cast the spell
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <item>
        /// <term>-2</term>
        /// <description>Health</description>
        /// </item>
        /// <item>
        /// <term>0</term>
        /// <description>Mana</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>Rage</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Focus</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Energy</description>
        /// </item>
        /// <item>
        /// <term>5</term>
        /// <description>Runes</description>
        /// </item>
        /// <item>
        /// <term>6</term>
        /// <description>Runic Power</description>
        /// </item>
        /// </list>
        /// </remarks>
        public readonly int PowerType;
        /// <summary>
        /// Casting time of the spell in milliseconds
        /// </summary>
        public readonly int CastTime;
        /// <summary>
        /// Minimum range from the target required to cast the spell
        /// </summary>
        public readonly int MinRange;
        /// <summary>
        /// Maximum range from the target at which you can cast the spell
        /// </summary>
        public readonly int MaxRange;
    }
}
