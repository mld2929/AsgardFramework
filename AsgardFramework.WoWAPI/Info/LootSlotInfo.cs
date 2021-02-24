using System.Runtime.InteropServices;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Info
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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct LootSlotInfoRaw
    {
        public readonly int Texture;
        public readonly int NameOrDescription;
        public readonly int Quantity;

        [MarshalAs(UnmanagedType.I4)]
        public readonly ItemQuality Quality;

        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool Locked;
    }

    public class LootSlotInfo
    {
        #region Constructors

        internal LootSlotInfo(LootSlotInfoRaw raw, IGlobalMemory memory) {
            Texture = memory.ReadNullTerminatedString(raw.Texture, Encoding.UTF8);
            NameOrDescription = memory.ReadNullTerminatedString(raw.NameOrDescription, Encoding.UTF8);
            Quantity = raw.Quantity;
            Quality = raw.Quality;
            Locked = raw.Locked;
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        ///     <see langword="true" /> if the item is locked (preventing the player from looting it); otherwise
        ///     <see langword="false" />
        /// </summary>
        public readonly bool Locked;

        /// <summary>
        ///     Name of the item, or description of the amount of money
        /// </summary>
        public readonly string NameOrDescription;

        /// <summary>
        ///     Quality (rarity) of the item
        /// </summary>
        public readonly ItemQuality Quality;

        /// <summary>
        ///     Number of stacked items, or 0 for money
        /// </summary>
        public readonly int Quantity;

        /// <summary>
        ///     Path to an icon texture for the item or amount of money
        /// </summary>
        public readonly string Texture;

        #endregion Fields
    }
}