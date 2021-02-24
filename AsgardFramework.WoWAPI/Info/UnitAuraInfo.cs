using System.Runtime.InteropServices;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Info
{
    public enum AuraType
    {
        None,
        Curse,
        Disease,
        Magic,
        Poison
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct UnitAuraInfoRaw
    {
        public readonly int Name;
        public readonly int RankOrSecondaryText;
        public readonly int Icon;
        public readonly int Count;
        public readonly int Type;
        public readonly double Duration;
        public readonly double ExpiresAt;
        public readonly int CasterUnitMetaId;

        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool Stealable;

        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool ConsolidatedAtUI;

        public readonly int SpellId;
    }

    public class UnitAuraInfo
    {
        #region Fields

        /// <summary>
        ///     Unit which applied the aura. <see cref="string.Empty" /> if the casting unit (or its controller) has no unitID
        /// </summary>
        /// <remarks>
        ///     If the aura was applied by a unit that does not have a token but is controlled by one that does (e.g. a totem or
        ///     another player's vehicle), value is the controlling unit meta id.
        /// </remarks>
        public readonly string CasterUnitMetaId;

        /// <summary>
        ///     <see langword="true" /> if the aura is eligible for the 'consolidated' aura display in the default UI.
        /// </summary>
        public readonly bool ConsolidatedAtUI;

        /// <summary>
        ///     The number of times the aura has been applied
        /// </summary>
        public readonly int Count;

        /// <summary>
        ///     Total duration of the aura (in seconds)
        /// </summary>
        public readonly double Duration;

        /// <summary>
        ///     Time at which the aura will expire
        /// </summary>
        public readonly double ExpiresAt;

        /// <summary>
        ///     Path to an icon texture for the aura
        /// </summary>
        public readonly string Icon;

        /// <summary>
        ///     Name of the aura
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     Secondary text for the aura (often a rank; e.g. <c>"Rank 7"</c>)
        /// </summary>
        public readonly string RankOrSecondaryText;

        /// <summary>
        ///     Spell ID of the aura
        /// </summary>
        public readonly int SpellId;

        /// <summary>
        ///     <see langword="true" /> if the aura can be transferred to a player using the Spellsteal spell; otherwise
        ///     <see langword="false" />
        /// </summary>
        public readonly bool Stealable;

        /// <summary>
        ///     Type of aura (relevant for dispelling and certain other mechanics)
        /// </summary>
        public readonly AuraType Type;

        #endregion Fields

        #region Constructors

        internal UnitAuraInfo(UnitAuraInfoRaw raw, IGlobalMemory memory) {
            Name = memory.ReadNullTerminatedString(raw.Name, Encoding.UTF8);
            RankOrSecondaryText = memory.ReadNullTerminatedString(raw.RankOrSecondaryText, Encoding.UTF8);
            Icon = memory.ReadNullTerminatedString(raw.Icon, Encoding.UTF8);
            Count = raw.Count;

            Type = memory.ReadNullTerminatedString(raw.Type, Encoding.UTF8) switch {
                "Curse" => AuraType.Curse,
                "Disease" => AuraType.Disease,
                "Magic" => AuraType.Magic,
                "Poison" => AuraType.Poison,
                _ => AuraType.None
            };

            Duration = raw.Duration;
            ExpiresAt = raw.ExpiresAt;
            CasterUnitMetaId = memory.ReadNullTerminatedString(raw.CasterUnitMetaId, Encoding.UTF8);
            Stealable = raw.Stealable;
            ConsolidatedAtUI = raw.ConsolidatedAtUI;
            SpellId = raw.SpellId;
        }

        #endregion Constructors
    }
}