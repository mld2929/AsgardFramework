namespace AsgardFramework.WoWAPI.Info
{
    public class ItemSpellInfo
    {
        /// <summary>
        /// Name of the spell
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Secondary text associated with the spell
        /// </summary>
        /// <remarks>
        /// Often a rank, e.g. <c>"Rank 7"</c>; or the <see cref="string.Empty"/> if not applicable
        /// </remarks>
        public readonly string RankOrSecondaryText;
    }
}
