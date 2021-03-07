namespace AsgardFramework.WoWAPI.LuaData
{
    public sealed class ItemSpellInfo : LuaValue
    {
        #region Methods

        public override void Parse(string[] data) {
            Name = data[0];
            RankOrSecondaryText = data[1];
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Name of the spell
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Secondary text associated with the spell
        /// </summary>
        /// <remarks>
        ///     Often a rank, e.g. <c>"Rank 7"</c>; or the <see cref="string.Empty" /> if not applicable
        /// </remarks>
        public string RankOrSecondaryText { get; private set; }

        #endregion Properties
    }
}