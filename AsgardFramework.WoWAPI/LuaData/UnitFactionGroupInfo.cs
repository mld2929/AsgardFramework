namespace AsgardFramework.WoWAPI.LuaData
{
    public class UnitFactionGroupInfo : LuaValue
    {
        #region Methods

        public override LuaValue Parse(string[] data) {
            Name = data[0];
            LocalizedName = data[1];

            return this;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     Localized name of the faction
        /// </summary>
        public string LocalizedName { get; private set; }

        /// <summary>
        ///     Non-localized (English) faction name of the faction (<c>"Horde"</c> or <c>"Alliance"</c>)
        /// </summary>
        public string Name { get; private set; }

        #endregion Properties
    }
}