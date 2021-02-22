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

        // I wasted about 6 hours to create size-prefixed structure, size-prefixed utf8 string inside that structure,
        // asm code for copy strings and fill sizes, configure LuaVMWrapper, custom marshaler, etc.
        // and after all I noticed (by exception) that custom marshalers can't be appended to fields, fuck.
        // Maybe I'll try again, e.g. I can create structure with string inside it, and append another
        // marshaler to that structure.
        // But fuck that shit I just want to have only C# string inside API classes without any custom types and
        // mapping code which must be written for each provided type.
        // I fucking hate myself and microsoft. ლ(ಠ益ಠლ)
        // C++ wait for me
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