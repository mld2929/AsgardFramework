namespace AsgardFramework.WoWAPI.LuaData
{
    public enum AuraType
    {
        None,
        Curse,
        Disease,
        Magic,
        Poison
    }

    public class UnitAuraInfo : LuaValue
    {
        #region Methods

        public override LuaValue Parse(string[] data) {
            Name = data[0];
            RankOrSecondaryText = data[1];
            Icon = data[2];

            Count = data[3]
                .ToInt();

            Type = data[4]
                .ToEnum<AuraType>();

            Duration = data[5]
                .ToDouble();

            ExpiresAt = data[6]
                .ToDouble();

            CasterUnitMetaId = data[7];

            Stealable = data[8]
                .ToBool();

            ConsolidatedAtUI = data[9]
                .ToBool();

            SpellId = data[10]
                .ToInt();

            return this;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Unit which applied the aura. <see cref="string.Empty" /> if the casting unit (or its controller) has no unitID
        /// </summary>
        /// <remarks>
        ///     If the aura was applied by a unit that does not have a token but is controlled by one that does (e.g. a totem or
        ///     another player's vehicle), value is the controlling unit meta id.
        /// </remarks>
        public string CasterUnitMetaId { get; private set; }

        /// <summary>
        ///     <see langword="true" /> if the aura is eligible for the 'consolidated' aura display in the default UI.
        /// </summary>
        public bool ConsolidatedAtUI { get; private set; }

        /// <summary>
        ///     The number of times the aura has been applied
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Total duration of the aura (in seconds)
        /// </summary>
        public double Duration { get; private set; }

        /// <summary>
        ///     Time at which the aura will expire
        /// </summary>
        public double ExpiresAt { get; private set; }

        /// <summary>
        ///     Path to an icon texture for the aura
        /// </summary>
        public string Icon { get; private set; }

        /// <summary>
        ///     Name of the aura
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Secondary text for the aura (often a rank; e.g. <c>"Rank 7"</c>)
        /// </summary>
        public string RankOrSecondaryText { get; private set; }

        /// <summary>
        ///     Spell ID of the aura
        /// </summary>
        public int SpellId { get; private set; }

        /// <summary>
        ///     <see langword="true" /> if the aura can be transferred to a player using the Spellsteal spell; otherwise
        ///     <see langword="false" />
        /// </summary>
        public bool Stealable { get; private set; }

        /// <summary>
        ///     Type of aura (relevant for dispelling and certain other mechanics)
        /// </summary>
        public AuraType Type { get; private set; }

        #endregion Properties
    }
}