using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x1268)]
    public sealed class Player : Unit
    {
        [FieldOffset(4200)]
        public readonly uint AmmoID;

        [FieldOffset(-0x238 - 0x18 + 0x13F8)]
        public readonly uint ArenaCurrency;

        [FieldOffset(-0x238 - 0x18 + 0x13A0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x15)]
        public readonly uint[] ArenaTeamInfo;

        [FieldOffset(-0x238 - 0x18 + 0x5C8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
        public readonly ulong[] BackpackItems;

        [FieldOffset(-0x238 - 0x18 + 0x728)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly ulong[] BankBags;

        [FieldOffset(-0x238 - 0x18 + 0x648)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x1C)]
        public readonly ulong[] BankItems;

        [FieldOffset(-0x238 - 0x18 + 0x1000)]
        public readonly float BlockPercentage;

        [FieldOffset(-0x238 - 0x18 + 0x12C4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC)]
        public readonly uint[] BuyBackPrice;

        [FieldOffset(-0x238 - 0x18 + 0x12F4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC)]
        public readonly uint[] BuyBackTimestamp;

        [FieldOffset(-0x238 - 0x18 + 0x264)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3)]
        public readonly uint[] Bytes;

        [FieldOffset(-0x238 - 0x18 + 0xFF0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x2)]
        public readonly uint[] CharacterPoints;

        [FieldOffset(-0x238 - 0x18 + 0x504)]
        public readonly uint ChosenTitle;

        [FieldOffset(-0x238 - 0x18 + 0x1248)]
        public readonly uint Coinage;

        [FieldOffset(-0x238 - 0x18 + 0x133C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x19)]
        public readonly uint[] CombatRating;

        [FieldOffset(-0x238 - 0x18 + 0x1014)]
        public readonly float CritPersentage;

        [FieldOffset(-0x238 - 0x18 + 0x8C0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public readonly ulong[] CurrencyToken;

        [FieldOffset(-0x238 - 0x18 + 0x1400)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x19)]
        public readonly uint[] DailyQuests;

        [FieldOffset(-0x238 - 0x18 + 0x1004)]
        public readonly float DodgePercentage;

        [FieldOffset(-0x238 - 0x18 + 0x250)]
        public readonly ulong DuelArbiter;

        [FieldOffset(-0x238 - 0x18 + 0x100C)]
        public readonly uint Expertise; // maybe float

        [FieldOffset(-0x238 - 0x18 + 0x508)]
        public readonly uint FakeInebriation;

        [FieldOffset(-0x238 - 0x18 + 0x9C0)]
        public readonly ulong Farsight;

        [FieldOffset(-0x238 - 0x18 + 0x1498)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x6)]
        public readonly uint[] Glyphs;

        [FieldOffset(-0x238 - 0x18 + 0x14B0)]
        public readonly uint GlyphsEnabled;

        [FieldOffset(-0x238 - 0x18 + 0x1480)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x6)]
        public readonly uint[] GlyphSlot;

        [FieldOffset(-0x238 - 0x18 + 0x25C)]
        public readonly uint GuildID;

        [FieldOffset(-0x238 - 0x18 + 0x260)]
        public readonly uint GuildRank;

        [FieldOffset(-0x238 - 0x18 + 0x274)]
        public readonly uint GuildTimestamp;

        [FieldOffset(-0x238 - 0x18 + 0x13F4)]
        public readonly uint HonorCurrency;

        [FieldOffset(-0x238 - 0x18 + 0x510)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x17)]
        public readonly ulong[] Inventory;

        [FieldOffset(-0x238 - 0x18 + 0x7C0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
        public readonly ulong[] Keyring;

        [FieldOffset(-0x238 - 0x18 + 0x1324)]
        public readonly uint Kills;

        [FieldOffset(-0x238 - 0x18 + 0x9E0)]
        public readonly ulong KnownCurrencies; // bit array or smth

        [FieldOffset(-0x238 - 0x18 + 0x9C8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3)]
        public readonly ulong[] KnownTitles; // bit array or smth

        [FieldOffset(-0x238 - 0x18 + 0x1330)]
        public readonly uint LifetimeHonarbaleKills;

        [FieldOffset(-0x238 - 0x18 + 0x13FC)]
        public readonly uint MaxLevel;

        [FieldOffset(-0x238 - 0x18 + 0x1268)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] ModDamageDoneNeg;

        [FieldOffset(-0x238 - 0x18 + 0x1284)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] ModDamageDonePct;

        [FieldOffset(-0x238 - 0x18 + 0x124C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] ModDamageDonePos;

        [FieldOffset(-0x238 - 0x18 + 0x12A8)]
        public readonly float ModHealingDonePct;

        [FieldOffset(-0x238 - 0x18 + 0x12A0)]
        public readonly float ModHealingDonePos;

        [FieldOffset(-0x238 - 0x18 + 0x12A4)]
        public readonly float ModHealingPct;

        [FieldOffset(-0x238 - 0x18 + 0x12B0)]
        public readonly float ModTargetPhysicalResistance;

        [FieldOffset(-0x238 - 0x18 + 0x12AC)]
        public readonly float ModTargetResistance;

        [FieldOffset(-0x238 - 0x18 + 0x9EC)]
        public readonly uint NextLevelXP;

        [FieldOffset(-0x238 - 0x18 + 0x1474)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3)]
        public readonly uint[] NoReagentCost;

        [FieldOffset(-0x238 - 0x18 + 0x101C)]
        public readonly float OffhandCritPersentage;

        [FieldOffset(-0x238 - 0x18 + 0x1010)]
        public readonly uint OffhandExpertise; // maybe float

        [FieldOffset(-0x238 - 0x18 + 0x1008)]
        public readonly float ParryPercentage;

        [FieldOffset(-0x238 - 0x18 + 0x14B4)]
        public readonly uint PetSpellPower;

        [FieldOffset(-0x238 - 0x18 + 0x12B4)]
        public readonly uint PlayerBytes;

        [FieldOffset(-0x238 - 0x18 + 0x1334)]
        public readonly uint PlayerBytes_2;

        [FieldOffset(-0x238 - 0x18 + 0x270)]
        public readonly uint PlayerDuelTeam;

        [FieldOffset(-0x238 - 0x18 + 0x258)]
        public readonly uint PlayerFlags;

        [FieldOffset(-0x238 - 0x18 + 0x12C0)]
        public readonly uint PvPMedals;

        [FieldOffset(-0x238 - 0x18 + 0x278)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7D)]
        public readonly uint[] QuestLog;

        [FieldOffset(-0x238 - 0x18 + 0x1018)]
        public readonly float RangedCritPersentage;

        [FieldOffset(-0x238 - 0x18 + 0x1244)]
        public readonly uint RestStateXP;

        [FieldOffset(-0x238 - 0x18 + 0x1464)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x4)]
        public readonly uint[] RuneRegen;

        [FieldOffset(-0x238 - 0x18 + 0x12BC)]
        public readonly uint SelfResSpell;

        [FieldOffset(-0x238 - 0x18 + 0x103C)]
        public readonly uint ShieldBlock;

        [FieldOffset(-0x238 - 0x18 + 0x1040)]
        public readonly float ShieldBlockCritPersentage;

        [FieldOffset(-0x238 - 0x18 + 0x9F0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x180)]
        public readonly uint[] SkillInfo;

        [FieldOffset(-0x238 - 0x18 + 0x1020)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] SpellCritPersentage;

        [FieldOffset(-0x238 - 0x18 + 0x1328)]
        public readonly uint TodayContribution;

        [FieldOffset(-0x238 - 0x18 + 0xFF8)]
        public readonly uint TrackCreatures; // bit array or smth

        [FieldOffset(-0x238 - 0x18 + 0xFFC)]
        public readonly uint TrackResources; // bit array or smth

        [FieldOffset(-0x238 - 0x18 + 0x760)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xC)]
        public readonly ulong[] VendorBuyback;

        [FieldOffset(-0x238 - 0x18 + 0x46C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x26)]
        public readonly uint[] VisibleItemEntryIDAndEnchantment;

        [FieldOffset(-0x238 - 0x18 + 0x1338)]
        public readonly uint WatchedFactionIndex;

        [FieldOffset(-0x238 - 0x18 + 0x9E8)]
        public readonly uint XP;

        [FieldOffset(-0x238 - 0x18 + 0x132C)]
        public readonly uint YesterdayContribution;

        [FieldOffset(-0x238 - 0x18 + 0x1044)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public ushort[] ExploredZones;
    }
}