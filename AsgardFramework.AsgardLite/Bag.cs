using System.Collections.Generic;

using AsgardFramework.WoWAPI.LuaData;
using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.AsgardLite
{
    public class Bag
    {
        #region Constructors

        internal Bag(IReadOnlyList<Item> items, int totalSlots, BagType type, string name) {
            TotalSlots = totalSlots;
            FreeSlots = TotalSlots - items.Count;
            Items = items;
            Type = type;
            Name = name;
        }

        #endregion Constructors

        #region Fields

        public readonly int FreeSlots;

        public readonly IReadOnlyList<Item> Items;

        public readonly string Name;

        public readonly int TotalSlots;

        public readonly BagType Type;

        #endregion Fields
    }
}