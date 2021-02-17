using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Object
    {
        public readonly ulong Guid;
        public readonly int ObjectType;
        public readonly uint EntryID;
        public readonly float Scale;
        private readonly uint padding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Item : Object
    {
        public readonly ulong Owner;
        public readonly ulong Contained;
        public readonly ulong Creator;
        public readonly ulong GiftCreator;
        public readonly uint StackCount;
        public readonly uint Duration;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public readonly uint[] SpellCharges;
        public readonly uint Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public readonly uint[] Enchantments;
        public readonly uint PropertySeed;
        public readonly uint RandomPropertiesID;
        public readonly uint Durability;
        public readonly uint MaxDurability;
        public readonly uint CreatePlayedTime;
        private readonly uint ItemPadding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Container : Item
    {
        public readonly uint Slots;
        private readonly uint AlignPad;
        public readonly ulong FirstSlotGuid;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class GameObject : Object
    {
        public readonly ulong CreatedBy;
        public readonly uint DisplayID;
        public readonly uint Flags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public readonly uint[] ParentRotation;
        public readonly uint Dynamic;
        public readonly uint Faction;
        public readonly uint Level;
        public readonly uint Bytes_1;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class DynamicObject : Object
    {
        public readonly ulong Caster;
        public readonly uint Bytes;
        public readonly uint SpellID;
        public readonly uint Radius;
        public readonly uint CastTime;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Corpse : Object
    {
        public readonly ulong Owner;
        public readonly ulong Party;
        public readonly uint DisplayID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
        public readonly ulong[] Items;
        public readonly uint Bytes_1;
        public readonly uint Bytes_2;
        public readonly uint Guild;
        public readonly uint Flags;
        public readonly uint DynamicFlags;
        public readonly uint CorpsePadding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Unit : Object
    {
        public readonly ulong Charm;
        public readonly ulong Summon;
        public readonly ulong Critter;
        public readonly ulong CharmedBy;
        public readonly ulong SummonnedBy;
        public readonly ulong CreatedBy;
        public readonly ulong Target;
        public readonly ulong ChannelObject;
        public readonly uint ChannelSpell;
        public readonly uint Bytes_0;
        public readonly uint Health;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly uint[] Power;
        public readonly uint MaxHealth;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly uint[] MaxPower;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] PowerRegenFlatModifier;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] PowerRegenInterruptedFlatModifier;
        public readonly uint Level;
        public readonly uint FactionTemplate;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public readonly uint[] VirtualItemSlotID;
        public readonly uint Flags;
        public readonly uint Flags_2;
        public readonly uint AuraState;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public readonly float[] BaseAttackTime;
        public readonly uint RangeAttackTime;
        public readonly float BoundingRadius;
        public readonly uint CombatReach;
        public readonly uint DisplayID;
        public readonly uint NativeDisplayID;
        public readonly uint MountDisplayID;
        public readonly float MinDamage;
        public readonly float MaxDamage;
        public readonly float MinOffhandDamage;
        public readonly float MaxOffhandDamage;
        public readonly uint Bytes_1;
        public readonly uint PetNumber;
        public readonly uint PetNameTimestamp;
        public readonly uint PetExperience;
        public readonly uint PetNexLevelExp;
        public readonly uint DynamicFlags;
        public readonly float ModCastSpeed;
        public readonly uint CreatedBySpell;
        public readonly uint NpcFlags;
        public readonly uint NpcEmoteState;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public readonly uint[] Stats;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public readonly uint[] PostStats;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public readonly uint[] NegStats;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] Resistances;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] ResistanceBuffModsPositive;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] ResistanceBuffModsNegative;
        public readonly uint BaseMana;
        public readonly uint BaseHealth;
        public readonly uint Bytes_2;
        public readonly uint AttackPower;
        public readonly float AttackPowerMods;
        public readonly float AttackPowerMultiplier;
        public readonly uint RangedAttackPower;
        public readonly float RangedAttackPowerMods;
        public readonly float RangedAttackPowerMultiplier;
        public readonly float MinRangedDamage;
        public readonly float MaxRangedDamage;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] PowerCostModifier;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] PowerCostMultiplier;
        public readonly float MaxHealthModifier;
        public readonly float HoverHeight;
        private readonly uint UnitPadding;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class Player : Unit
    {
        public readonly ulong DuelArbiter;
        public readonly uint PlayerFlags;
        public readonly uint GuildID;
        public readonly uint GuildRank;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public readonly uint[] Player_Bytes;
        public readonly uint PlayerDuelTeam;
        public readonly uint GuildTimestamp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 125)]
        public readonly uint[] QuestLog;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 38)]
        public readonly uint[] VisibleItemEntryIDAndEnchantment;
        public readonly uint ChosenTitle;
        public readonly uint FakeInebriation;
        private readonly uint PlayerPad_0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 23)]
        public readonly ulong[] Inventory;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public readonly ulong[] Backpack;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 28)]
        public readonly ulong[] Bank;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly ulong[] BankBag;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public readonly ulong[] VendorBuyback;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public readonly ulong[] Keyring;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public readonly ulong[] CurrencyToken;
        public readonly ulong Farsight;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public readonly ulong[] KnownTitles;
        public readonly ulong KnownCurrencies;
        public readonly uint XP;
        public readonly uint NextLevelXP;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 384)]
        public readonly uint[] SkillInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public readonly uint[] CharacterPoints;
        public readonly uint TrackCreatures;
        public readonly uint TrackResources;
        public readonly float BlockPercentage;
        public readonly float DodgePercentage;
        public readonly float ParryPercentage;
        public readonly uint Expertise;
        public readonly uint OffhandExpertise;
        public readonly float CritPersentage;
        public readonly float RangedCritPersentage;
        public readonly float OffhandCritPersentage;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] SpellCritPersentage;
        public readonly uint ShieldBlock;
        public readonly float ShieldBlockCritPersentage;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public ushort[] ExploredZones;
        public readonly uint RestStateXP;
        public readonly uint Coinage;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] ModDamageDonePos;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] ModDamageDoneNeg;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        public readonly float[] ModDamageDonePct;
        public readonly float ModHealingDonePos;
        public readonly float ModHealingPct;
        public readonly float ModHealingDonePct;
        public readonly float ModTargetResistance;
        public readonly float ModTargetPhysicalResistance;
        public readonly uint PlayerBytes;
        public readonly uint AmmoID;
        public readonly uint SelfResSpell;
        public readonly uint PvPMedals;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public readonly uint[] BuyBackPrice;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public readonly uint[] BuyBackTimestamp;
        public readonly uint Kills;
        public readonly uint TodayContribution;
        public readonly uint YesterdayContribution;
        public readonly uint LifetimeHonarbaleKills;
        public readonly uint PlayerBytes_2;
        public readonly uint WatchedFactionIndex;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        public readonly uint[] CombatRating;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 21)]
        public readonly uint[] ArenaTeamInfo;
        public readonly uint HonorCurrency;
        public readonly uint ArenaCurrency;
        public readonly uint MaxLevel;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
        public readonly uint[] DailyQuests;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public readonly uint[] RuneRegen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public readonly uint[] NoReagentCost;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public readonly uint[] GlyphSlot;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public readonly uint[] Glyphs;
        public readonly uint GlyphsEnabled;
        public readonly uint PetSpellPower;
    }
}
