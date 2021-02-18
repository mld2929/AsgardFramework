namespace AsgardFramework.WoWAPI.Info
{
    public class UnitAuraInfo
    {
        /// <summary>
        /// Name of the aura
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Secondary text for the aura (often a rank; e.g. <c>"Rank 7"</c>)
        /// </summary>
        public readonly string RankOrSecondaryText;
        /// <summary>
        /// Path to an icon texture for the aura
        /// </summary>
        public readonly string Icon;
        /// <summary>
        /// The number of times the aura has been applied
        /// </summary>
        public readonly int Count;
        /// <summary>
        /// Type of aura (relevant for dispelling and certain other mechanics); <see cref="string.Empty"/> if not one of the following values:
        /// <list type="table">
        /// <item>
        /// <b>Curse</b>
        /// </item>
        /// <item>
        /// <b>Disease</b>
        /// </item>
        /// <item>
        /// <b>Magic</b>
        /// </item>
        /// <item>
        /// <b>Poison</b>
        /// </item>
        /// </list>
        /// </summary>
        public readonly string Type;
        /// <summary>
        /// Total duration of the aura (in seconds) 
        /// </summary>
        public readonly int Duration;
        /// <summary>
        /// Time at which the aura will expire
        /// </summary>
        public readonly float ExpiresAt;
        /// <summary>
        /// Unit which applied the aura. <see cref="string.Empty"/> if the casting unit (or its controller) has no unitID
        /// </summary>
        /// <remarks>
        /// If the aura was applied by a unit that does not have a token but is controlled by one that does (e.g. a totem or another player's vehicle), value is the controlling unit meta id.
        /// </remarks>
        public readonly string CasterUnitMetaId;
        /// <summary>
        /// <see langword="true"/> if the aura can be transferred to a player using the Spellsteal spell; otherwise <see langword="false"/>
        /// </summary>
        public readonly bool Stealable;
        /// <summary>
        /// <see langword="true"/> if the aura is eligible for the 'consolidated' aura display in the default UI.
        /// </summary>
        public readonly bool ConsolidatedAtUI;
        /// <summary>
        /// Spell ID of the aura
        /// </summary>
        public readonly int SpellId;
    }
}
