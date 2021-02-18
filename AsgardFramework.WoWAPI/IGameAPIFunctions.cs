using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Info;

namespace AsgardFramework.WoWAPI
{
    public interface IGameAPIFunctions
    {
        /// <summary>
        /// Returns the number of items available to be looted
        /// </summary>
        Task<int> GetNumLootItems();
        /// <summary>
        /// Attempts to pick up an item available as loot
        /// </summary>
        /// <param name="slotNumberFromOne">Index of a loot slot (between 1 and <see cref="GetNumLootItems"/>)</param>
        Task LootSlot(int slotNumberFromOne);
        /// <summary>
        /// Returns information about the contents of a loot slot. 
        /// </summary>
        Task<LootSlotInfo> GetLootSlotInfo(int slotNumberFromOne);
        /// <summary>
        /// Returns information about a spell
        /// </summary>
        /// <param name="spellId">Numeric ID of a spell</param>
        Task<SpellInfo> GetSpellInfo(int spellId);
        /// <summary>
        /// Returns information about a spell
        /// </summary>
        /// <param name="spellName">
        /// <br>Name of a spell, optionally including secondary text </br>
        /// <br>(e.g. <c>"Mana Burn"</c> to find the player's highest rank, or <c>"Mana Burn(Rank 2)"</c> (no space before the parenthesis) for a specific rank) </br>
        /// </param>
        Task<SpellInfo> GetSpellInfo(string spellName);
        /// <summary>
        /// Returns information about the spell cast by an item's <c>"Use:"</c> effect
        /// </summary>
        /// <param name="itemName">An item's name</param>
        Task<ItemSpellInfo> GetItemSpell(string itemName);
        /// <summary>
        /// Returns information about the spell cast by an item's <c>"Use:"</c> effect
        /// </summary>
        /// <param name="itemId">An item's ID</param>
        Task<ItemSpellInfo> GetItemSpell(int itemId);
        /// <summary>
        /// Attempts to equip an arbitrary item.
        /// </summary>
        /// <remarks>
        /// The item is automatically equipped in the first available slot in which it fits
        /// </remarks>
        /// <param name="name">An item's name</param>
        Task EquipItem(string name);
        /// <summary>
        /// Attempts to equip an arbitrary item.
        /// </summary>
        /// <remarks>
        /// The item is automatically equipped in the first available slot in which it fits
        /// </remarks>
        /// <param name="itemId">An item's ID</param>
        Task EquipItem(int itemId);
        ///<summary>
        /// Returns a unit's class
        ///</summary>
        /// <param name="unitMetaIdOrName">Name only valid for player, pet, and party/raid members</param>
        Task<UnitClassInfo> GetUnitClass(string unitMetaIdOrName);
        /// <summary>
        /// Returns whether a unit is currently in combat
        /// </summary>
        /// <param name="unitMetaId">A unit to query</param>
        Task<bool> IsUnitAffectingCombat(string unitMetaId);
        /// <summary>
        /// Returns whether a unit is a player unit (not an NPC)
        /// </summary>
        /// <param name="unitMetaId">A unit to query</param>
        Task<bool> IsPlayer(string unitMetaId);
        /// <summary>
        /// Returns whether the player is currently falling
        /// </summary>
        Task<bool> IsFalling();
        /// <summary>
        /// Returns whether the player is mounted
        /// </summary>
        Task<bool> IsMounted();
        /// <summary>
        /// Returns whether the player character can be controlled
        /// </summary>
        Task<bool> HasFullControl();
        /// <summary>
        /// Returns cooldown information about a spell
        /// </summary>
        /// <param name="name">Name of a spell</param>
        Task<SpellCooldownInfo> GetSpellCooldown(string name);
        /// <summary>
        /// Returns cooldown information about a spell
        /// </summary>
        /// <param name="spellId">Numeric ID of a spell</param>
        Task<SpellCooldownInfo> GetSpellCooldown(int spellId);
        /// <summary>
        /// Returns whether the login process has completed
        /// </summary>
        Task<bool> IsLoggedIn();
        /// <summary>
        /// Returns whether the player is currently outdoors
        /// </summary>
        Task<bool> IsOutdoors();
        /// <summary>
        /// Returns information about buffs/debuffs on a unit
        /// </summary>
        /// <param name="unitMetaId">A unit to query</param>
        /// <param name="index">Index of an aura to query</param>
        /// <param name="filter">
        /// <br>A list of filters to use separated by the pipe <c>|</c> character</br>
        /// <br>e.g. <c>"RAID|PLAYER"</c> will query group buffs cast by the player</br>
        /// <list type="table">
        /// <item>
        /// <term>CANCELABLE</term>
        /// <description>Show auras that can be cancelled</description>
        /// </item>
        /// <item>
        /// <term>HARMFUL</term>
        /// <description>Show debuffs only</description>
        /// </item>
        /// <item>
        /// <term>HELPFUL</term>
        /// <description>Show buffs only</description>
        /// </item>
        /// <item>
        /// <term>NOT_CANCELABLE</term>
        /// <description>Show auras that cannot be cancelled</description>
        /// </item>
        /// <item>
        /// <term>PLAYER</term>
        /// <description>Show auras the player has cast</description>
        /// </item>
        /// <item>
        /// <term>RAID</term>
        /// <description>
        /// When used with a <c>HELPFUL</c> filter it will show auras the player can cast on party/raid members (as opposed to self buffs).
        /// <br>If used with a <c>HARMFUL</c> filter it will return debuffs the player can cure</br>
        /// </description>
        /// </item>
        /// </list>
        /// </param>
        Task<UnitAuraInfo> GetUnitAura(string unitMetaId, int index, string filter = null);
        /// <summary>
        /// Returns information about buffs/debuffs on a unit
        /// </summary>
        /// <param name="unitMetaId">A unit to query</param>
        /// <param name="auraName">Name of an aura to query</param>
        /// <param name="auraSecondaryText">Secondary text of an aura to query (often a rank; e.g. <c>"Rank 7"</c>)</param>
        /// <param name="filter">
        /// <br>A list of filters to use separated by the pipe <c>|</c> character</br>
        /// <br>e.g. <c>"RAID|PLAYER"</c> will query group buffs cast by the player</br>
        /// <list type="table">
        /// <item>
        /// <term>CANCELABLE</term>
        /// <description>Show auras that can be cancelled</description>
        /// </item>
        /// <item>
        /// <term>HARMFUL</term>
        /// <description>Show debuffs only</description>
        /// </item>
        /// <item>
        /// <term>HELPFUL</term>
        /// <description>Show buffs only</description>
        /// </item>
        /// <item>
        /// <term>NOT_CANCELABLE</term>
        /// <description>Show auras that cannot be cancelled</description>
        /// </item>
        /// <item>
        /// <term>PLAYER</term>
        /// <description>Show auras the player has cast</description>
        /// </item>
        /// <item>
        /// <term>RAID</term>
        /// <description>
        /// When used with a <c>HELPFUL</c> filter it will show auras the player can cast on party/raid members (as opposed to self buffs).
        /// <br>If used with a <c>HARMFUL</c> filter it will return debuffs the player can cure</br>
        /// </description>
        /// </item>
        /// </list>
        /// </param>
        Task<UnitAuraInfo> GetUnitAura(string unitMetaId, string auraName, string auraSecondaryText = null, string filter = null);
        /// <summary>
        /// Returns a unit's primary faction allegiance
        /// </summary>
        /// <param name="unitMetaIdOrName">The name of a unit to query only valid for player, pet, and party/raid members</param>
        Task<UnitFactionGroupInfo> UnitFactionGroup(string unitMetaIdOrName);
        /// <summary>
        /// Causes the player character to automatically follow another unit. Only friendly player units can be followed.
        /// </summary>
        /// <param name="unitMetaIdOrName">A unit to follow</param>
        Task StartFollowUnit(string unitMetaIdOrName);
        /// <summary>
        /// Begins auto-attack against the player's current target. (If the "Auto Attack/Auto Shot" option is turned on, also begins Auto Shot for hunters.)
        /// </summary>
        Task AttackTarget();
        /// <summary>
        /// Dismounts from the player's summoned mount
        /// </summary>
        Task Dismount();
        /// <summary>
        /// Runs a string as a Lua script. Protected functions are available
        /// </summary>
        Task RunScript(string luaScript);
    }
}
