namespace AsgardFramework.WoWAPI.LuaData
{
    public enum ItemQuality
    {
        Poor,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Artifact,
        Heirloom
    }

    public class LootSlotInfo : LuaValue
    {
        #region Methods

        public override LuaValue Parse(string[] data) {
            Texture = data[0];
            NameOrDescription = data[1];

            Quantity = data[2]
                .ToInt();

            Quality = data[3]
                .ToEnum<ItemQuality>();

            Locked = data[4]
                .ToBool();

            return this;
        }

        #endregion Methods

        #region Properties

        /// <summary>
        ///     <see langword="true" /> if the item is locked (preventing the player from looting it); otherwise
        ///     <see langword="false" />
        /// </summary>
        public bool Locked { get; private set; }

        /// <summary>
        ///     Name of the item, or description of the amount of money
        /// </summary>
        public string NameOrDescription { get; private set; }

        /// <summary>
        ///     Quality (rarity) of the item
        /// </summary>
        public ItemQuality Quality { get; private set; }

        /// <summary>
        ///     Number of stacked items, or 0 for money
        /// </summary>
        public int Quantity { get; private set; }

        /// <summary>
        ///     Path to an icon texture for the item or amount of money
        /// </summary>
        public string Texture { get; private set; }

        #endregion Properties
    }
}