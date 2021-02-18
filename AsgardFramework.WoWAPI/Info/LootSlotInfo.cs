using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LootSlotInfo
    {
        /// <summary>
        /// Path to an icon texture for the item or amount of money
        /// </summary>
        public readonly string Texture;
        /// <summary>
        /// Name of the item, or description of the amount of money 
        /// </summary>
        public readonly string NameOrDescription;
        /// <summary>
        /// Number of stacked items, or 0 for money
        /// </summary>
        public readonly int Quantity;
        /// <summary>
        /// Quality (rarity) of the item
        /// </summary>
        /// <remarks>
        /// <list type="table">
        /// <item>
        /// <term>0</term>
        /// <description>Poor (gray)</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>Common (white)</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>Uncommon (green)</description>
        /// </item>
        /// <item>
        /// <term>3</term>
        /// <description>Rare/Superior (blue)</description>
        /// </item>
        /// <item>
        /// <term>4</term>
        /// <description>Epic (purple)</description>
        /// </item>
        /// <item>
        /// <term>5</term>
        /// <description>Legendary (orange)</description>
        /// </item>
        /// <item>
        /// <term>6</term>
        /// <description>Artifact (golden yellow)</description>
        /// </item>
        /// <item>
        /// <term>7</term>
        /// <description>Heirloom (light yellow)</description>
        /// </item>
        /// </list>
        /// </remarks>
        public readonly int Quality;
        /// <summary>
        /// <see langword="true"/> if the item is locked (preventing the player from looting it); otherwise <see langword="false"/>
        /// </summary>
        public readonly bool Locked;
    }
}
