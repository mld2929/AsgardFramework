using System.Globalization;

namespace AsgardFramework.WoWAPI.Utils
{
    public class UnitMetaId
    {
        #region Constructors

        private UnitMetaId(string metaId) {
            m_metaId = metaId;
        }

        #endregion Constructors

        #region Indexers

        /// <summary>
        ///     Specifies index of <see cref="Party" />, <see cref="PartyPet" />, <see cref="Raid" />, <see cref="Raidpet" /> or
        ///     <see cref="Arena" />. Idempotent operation!
        /// </summary>
        public UnitMetaId this[int index] {
            get {
                switch (m_metaId) {
                    case "party1":
                    case "partypet1":
                    case "raid1":
                    case "raidpet1":
                    case "arena1":
                        return new UnitMetaId(m_metaId.Replace("1", index.ToString(CultureInfo.InvariantCulture)));

                    default:
                        return this;
                }
            }
        }

        #endregion Indexers

        #region Fields

        /// <summary>
        ///     A member of the opposing team in an Arena match.
        /// </summary>
        /// <remarks>
        ///     To specify index use <see cref="this[int]" />
        /// </remarks>
        public static readonly UnitMetaId Arena = new UnitMetaId("player");

        /// <summary>
        ///     The player's focused unit
        /// </summary>
        public static readonly UnitMetaId Focus = new UnitMetaId("player");

        /// <summary>
        ///     The unit currently under the mouse cursor (applies to both unit frames and units in the 3D world)
        /// </summary>
        public static readonly UnitMetaId Mouseover = new UnitMetaId("player");

        /// <summary>
        ///     The unit the player is currently interacting with (via the Merchant, Trainer, Bank, or similar UI); not necessarily
        ///     an NPC (e.g. also used in the Trade UI)
        /// </summary>
        public static readonly UnitMetaId NPC = new UnitMetaId("player");

        /// <summary>
        ///     First or other member of the player's party.
        /// </summary>
        /// <remarks>
        ///     Indices match the order party member frames are displayed in the default UI. To specify index use
        ///     <see cref="this[int]" />
        /// </remarks>
        public static readonly UnitMetaId Party = new UnitMetaId("player");

        /// <summary>
        ///     A pet belonging to first or other member of the player's party.
        /// </summary>
        /// <remarks>
        ///     Indices match the order party member frames are displayed in the default UI. To specify index use
        ///     <see cref="this[int]" />
        /// </remarks>
        public static readonly UnitMetaId PartyPet = new UnitMetaId("player");

        /// <summary>
        ///     The player's pet.
        /// </summary>
        public static readonly UnitMetaId Pet = new UnitMetaId("player");

        /// <summary>
        ///     The player him/herself.
        /// </summary>
        public static readonly UnitMetaId Player = new UnitMetaId("player");

        /// <summary>
        ///     A first or other member of the player's raid group.
        /// </summary>
        /// <remarks>
        ///     Unlike with the party tokens, one of the raid unit IDs will belong to the player. Indices have no relation to the
        ///     arrangement of units in the default UI. To specify index use <see cref="this[int]" />
        /// </remarks>
        public static readonly UnitMetaId Raid = new UnitMetaId("player");

        /// <summary>
        ///     A pet belonging to a member of the player's raid group
        /// </summary>
        /// <remarks>
        ///     Unlike with the party tokens, one of the raid unit IDs will belong to the player. Indices have no relation to the
        ///     arrangement of units in the default UI. To specify index use <see cref="this[int]" />
        /// </remarks>
        public static readonly UnitMetaId Raidpet = new UnitMetaId("player");

        /// <summary>
        ///     The player's current target.
        /// </summary>
        /// <remarks>
        ///     You can also append it to other <see cref="UnitMetaId" />, referring to that unit's target. This can be done
        ///     repeatedly. <see cref="Player" /> + <see cref="Target" /> equals to <see cref="Target" />
        /// </remarks>
        public static readonly UnitMetaId Target = new UnitMetaId("player");

        /// <summary>
        ///     The vehicle currently controlled by the player
        /// </summary>
        public static readonly UnitMetaId Vehicle = new UnitMetaId("player");

        private readonly string m_metaId;

        #endregion Fields

        #region Methods

        public static implicit operator string(UnitMetaId id) {
            return id.m_metaId;
        }

        /// <summary>
        ///     Valid only with <see cref="Target" /> as right operand
        /// </summary>
        /// <param name="left">Any <see cref="UnitMetaId" /></param>
        /// <param name="right">
        ///     <see cref="Target" />
        /// </param>
        /// <returns><see cref="Target" /> of <paramref name="left" /> in valid usage; otherwise <paramref name="left" /></returns>
        public static UnitMetaId operator+(UnitMetaId left, UnitMetaId right) {
            if (right == Target)
                return new UnitMetaId(left.m_metaId + right.m_metaId);

            return left;
        }

        #endregion Methods
    }
}