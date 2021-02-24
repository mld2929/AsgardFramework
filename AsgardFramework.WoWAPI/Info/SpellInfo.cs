using System.Runtime.InteropServices;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Info
{
    public enum PowerType
    {
        Health = -2,
        Mana = 0,
        Rage,
        Focus,
        Energy,
        Runes = 5,
        RunicPower
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SpellInfoRaw
    {
        /// <summary>
        ///     Name of the spell
        /// </summary>
        public readonly int Name;

        /// <summary>
        ///     Secondary text associated with the spell (e.g.<c>"Rank 5"</c>, <c>"Racial"</c>, etc.)
        /// </summary>
        public readonly int RankOrSecondaryText;

        /// <summary>
        ///     Path to an icon texture for the spel
        /// </summary>
        public readonly int Icon;

        /// <summary>
        ///     Amount of mana, rage, energy, runic power, or focus required to cast the spell
        /// </summary>
        public readonly double Cost;

        /// <summary>
        ///     <see langword="true" /> for spells with health funneling effects (like Health Funnel)
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool IsFunnel;

        /// <summary>
        ///     Power type to cast the spell
        /// </summary>
        [MarshalAs(UnmanagedType.I4)]
        public readonly PowerType PowerType;

        /// <summary>
        ///     Casting time of the spell in milliseconds
        /// </summary>
        public readonly int CastTime;

        /// <summary>
        ///     Minimum range from the target required to cast the spell
        /// </summary>
        public readonly double MinRange;

        /// <summary>
        ///     Maximum range from the target at which you can cast the spell
        /// </summary>
        public readonly double MaxRange;
    }

    public class SpellInfo
    {
        #region Constructors

        internal SpellInfo(SpellInfoRaw raw, IGlobalMemory memory) {
            Name = memory.ReadNullTerminatedString(raw.Name, Encoding.UTF8);
            RankOrSecondaryText = memory.ReadNullTerminatedString(raw.RankOrSecondaryText, Encoding.UTF8);
            Icon = memory.ReadNullTerminatedString(raw.Icon, Encoding.UTF8);
            Cost = raw.Cost;
            IsFunnel = raw.IsFunnel;
            PowerType = raw.PowerType;
            CastTime = raw.CastTime;
            MinRange = raw.MinRange;
            MaxRange = raw.MaxRange;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     Casting time of the spell in milliseconds
        /// </summary>
        public readonly int CastTime;

        /// <summary>
        ///     Amount of mana, rage, energy, runic power, or focus required to cast the spell
        /// </summary>
        public readonly double Cost;

        /// <summary>
        ///     Path to an icon texture for the spel
        /// </summary>
        public readonly string Icon;

        /// <summary>
        ///     <see langword="true" /> for spells with health funneling effects (like Health Funnel)
        /// </summary>
        public readonly bool IsFunnel;

        /// <summary>
        ///     Maximum range from the target at which you can cast the spell
        /// </summary>
        public readonly double MaxRange;

        /// <summary>
        ///     Minimum range from the target required to cast the spell
        /// </summary>
        public readonly double MinRange;

        /// <summary>
        ///     Name of the spell
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     Power type to cast the spell
        /// </summary>
        public readonly PowerType PowerType;

        /// <summary>
        ///     Secondary text associated with the spell (e.g.<c>"Rank 5"</c>, <c>"Racial"</c>, etc.)
        /// </summary>
        public readonly string RankOrSecondaryText;

        #endregion Fields
    }
}