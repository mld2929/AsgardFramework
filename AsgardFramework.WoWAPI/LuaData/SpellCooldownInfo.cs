namespace AsgardFramework.WoWAPI.LuaData
{
    public class SpellCooldownInfo : LuaValue
    {
        #region Methods

        public override LuaValue Parse(string[] data) {
            Start = data[0]
                .ToDouble();

            Duration = data[1]
                .ToDouble();

            CooldownVisibleAtUI = data[2]
                .ToBool();

            return this;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     <see langword="true" /> if a Cooldown UI element should be used to display the cooldown, otherwise
        ///     <see langword="false" />.
        /// </summary>
        /// <remarks>
        ///     Does not always correlate with whether the spell is ready.
        /// </remarks>
        public bool CooldownVisibleAtUI { get; private set; }

        /// <summary>
        ///     The length of the cooldown, or 0 if the spell is ready
        /// </summary>
        public double Duration { get; private set; }

        /// <summary>
        ///     Time in seconds (with millisecond precision) at the moment the cooldown began, or 0 if the spell is ready
        /// </summary>
        public double Start { get; private set; }

        #endregion Properties
    }
}