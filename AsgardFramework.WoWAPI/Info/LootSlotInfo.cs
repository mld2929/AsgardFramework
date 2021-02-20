using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 20)]
    public class LootSlotInfo
    {
        /// <summary>
        ///     <see langword="true" /> if the item is locked (preventing the player from looting it); otherwise
        ///     <see langword="false" />
        /// </summary>
        [FieldOffset(16)]
        [MarshalAs(UnmanagedType.U4)]
        public readonly bool Locked;

        /// <summary>
        ///     Name of the item, or description of the amount of money
        /// </summary>
        [FieldOffset(4)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string NameOrDescription;

        /// <summary>
        ///     Quality (rarity) of the item
        /// </summary>
        /// <remarks>
        ///     <list type="table">
        ///         <item>
        ///             <term>0</term>
        ///             <description>Poor (gray)</description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>Common (white)</description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>Uncommon (green)</description>
        ///         </item>
        ///         <item>
        ///             <term>3</term>
        ///             <description>Rare/Superior (blue)</description>
        ///         </item>
        ///         <item>
        ///             <term>4</term>
        ///             <description>Epic (purple)</description>
        ///         </item>
        ///         <item>
        ///             <term>5</term>
        ///             <description>Legendary (orange)</description>
        ///         </item>
        ///         <item>
        ///             <term>6</term>
        ///             <description>Artifact (golden yellow)</description>
        ///         </item>
        ///         <item>
        ///             <term>7</term>
        ///             <description>Heirloom (light yellow)</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        [FieldOffset(12)]
        public readonly int Quality;

        /// <summary>
        ///     Number of stacked items, or 0 for money
        /// </summary>
        [FieldOffset(8)]
        public readonly int Quantity;

        /// <summary>
        ///     Path to an icon texture for the item or amount of money
        /// </summary>
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.LPUTF8Str)]
        public readonly string Texture;
    }
}