namespace AsgardFramework.WoWAPI.LuaData
{
    public class UnitClassInfo : LuaValue
    {
        #region Methods

        public override void Parse(string[] data) {
            LocalizedName = data[0];
            Name = data[1];
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     The localized name of the unit's class, or the unit's name if the unit is an NPC
        /// </summary>
        public string LocalizedName { get; private set; }

        /// <summary>
        ///     A non-localized token representing the class
        /// </summary>
        public string Name { get; private set; }

        #endregion Properties
    }
}