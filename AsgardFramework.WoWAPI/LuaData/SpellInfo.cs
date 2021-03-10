namespace AsgardFramework.WoWAPI.LuaData
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

    public class SpellInfo : LuaValue
    {
        #region Methods

        public override LuaValue Parse(string[] data) {
            Name = data[0];
            RankOrSecondaryText = data[1];
            Icon = data[2];

            Cost = data[3]
                .ToDouble();

            IsFunnel = data[4]
                .ToBool();

            PowerType = data[5]
                .ToEnum<PowerType>();

            CastTime = data[6]
                .ToInt();

            MinRange = data[7]
                .ToDouble();

            MaxRange = data[8]
                .ToDouble();

            return this;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Casting time of the spell in milliseconds
        /// </summary>
        public int CastTime { get; private set; }

        /// <summary>
        ///     Amount of mana, rage, energy, runic power, or focus required to cast the spell
        /// </summary>
        public double Cost { get; private set; }

        /// <summary>
        ///     Path to an icon texture for the spel
        /// </summary>
        public string Icon { get; private set; }

        /// <summary>
        ///     <see langword="true" /> for spells with health funneling effects (like Health Funnel)
        /// </summary>
        public bool IsFunnel { get; private set; }

        /// <summary>
        ///     Maximum range from the target at which you can cast the spell
        /// </summary>
        public double MaxRange { get; private set; }

        /// <summary>
        ///     Minimum range from the target required to cast the spell
        /// </summary>
        public double MinRange { get; private set; }

        /// <summary>
        ///     Name of the spell
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Power type to cast the spell
        /// </summary>
        public PowerType PowerType { get; private set; }

        /// <summary>
        ///     Secondary text associated with the spell (e.g.<c>"Rank 5"</c>, <c>"Racial"</c>, etc.)
        /// </summary>
        public string RankOrSecondaryText { get; private set; }

        #endregion Properties
    }
}