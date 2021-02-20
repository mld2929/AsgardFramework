using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Objects
{
    [StructLayout(LayoutKind.Explicit, Pack = 0x1, Size = 0x238)]
    public class Unit : Object
    {
        [FieldOffset(-0x18 + 0x1EC)]
        public readonly uint AttackPower;

        [FieldOffset(-0x18 + 0x1F0)]
        public readonly float AttackPowerMods;

        [FieldOffset(-0x18 + 0x1F4)]
        public readonly float AttackPowerMultiplier;

        [FieldOffset(-0x18 + 0xF4)]
        public readonly uint AuraState;

        [FieldOffset(-0x18 + 0xF8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x2)]
        public readonly float[] BaseAttackTime;

        [FieldOffset(-0x18 + 0x1E4)]
        public readonly uint BaseHealth;

        [FieldOffset(-0x18 + 0x1E0)]
        public readonly uint BaseMana;

        [FieldOffset(-0x18 + 0x104)]
        public readonly float BoundingRadius;

        [FieldOffset(-0x18 + 0x5C)]
        public readonly uint Bytes_0;

        [FieldOffset(-0x18 + 0x128)]
        public readonly uint Bytes_1;

        [FieldOffset(-0x18 + 0x1E8)]
        public readonly uint Bytes_2;

        [FieldOffset(-0x18 + 0x50)]
        public readonly ulong ChannelObject;

        [FieldOffset(-0x18 + 0x58)]
        public readonly uint ChannelSpell;

        [FieldOffset(-0x18 + 0x18)]
        public readonly ulong Charm;

        [FieldOffset(-0x18 + 0x30)]
        public readonly ulong CharmedBy;

        [FieldOffset(-0x18 + 0x108)]
        public readonly uint CombatReach;

        [FieldOffset(-0x18 + 0x40)]
        public readonly ulong CreatedBy;

        [FieldOffset(-0x18 + 0x144)]
        public readonly uint CreatedBySpell;

        [FieldOffset(-0x18 + 0x28)]
        public readonly ulong Critter;

        [FieldOffset(-0x18 + 0x10C)]
        public readonly uint DisplayID;

        [FieldOffset(-0x18 + 0x13C)]
        public readonly uint DynamicFlags;

        [FieldOffset(-0x18 + 0xDC)]
        public readonly uint FactionTemplate;

        [FieldOffset(-0x18 + 0xEC)]
        public readonly uint Flags;

        [FieldOffset(-0x18 + 0xF0)]
        public readonly uint Flags_2;

        [FieldOffset(-0x18 + 0x60)]
        public readonly uint Health;

        [FieldOffset(-0x18 + 0x248)]
        public readonly float HoverHeight;

        [FieldOffset(-0x18 + 0xD8)]
        public readonly uint Level;

        [FieldOffset(-0x18 + 0x11C)]
        public readonly float MaxDamage;

        [FieldOffset(-0x18 + 0x80)]
        public readonly uint MaxHealth;

        [FieldOffset(-0x18 + 0x244)]
        public readonly float MaxHealthModifier;

        [FieldOffset(-0x18 + 0x124)]
        public readonly float MaxOffhandDamage;

        [FieldOffset(-0x18 + 0x84)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly uint[] MaxPower;

        [FieldOffset(-0x18 + 0x208)]
        public readonly float MaxRangedDamage;

        [FieldOffset(-0x18 + 0x118)]
        public readonly float MinDamage;

        [FieldOffset(-0x18 + 0x120)]
        public readonly float MinOffhandDamage;

        [FieldOffset(-0x18 + 0x204)]
        public readonly float MinRangedDamage;

        [FieldOffset(-0x18 + 0x140)]
        public readonly float ModCastSpeed;

        [FieldOffset(-0x18 + 0x114)]
        public readonly uint MountDisplayID;

        [FieldOffset(-0x18 + 0x110)]
        public readonly uint NativeDisplayID;

        [FieldOffset(-0x18 + 0x178)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5)]
        public readonly uint[] NegStats;

        [FieldOffset(-0x18 + 0x14C)]
        public readonly uint NpcEmoteState;

        [FieldOffset(-0x18 + 0x148)]
        public readonly uint NpcFlags;

        [FieldOffset(-0x18 + 0x134)]
        public readonly uint PetExperience;

        [FieldOffset(-0x18 + 0x130)]
        public readonly uint PetNameTimestamp;

        [FieldOffset(-0x18 + 0x138)]
        public readonly uint PetNexLevelExp;

        [FieldOffset(-0x18 + 0x12C)]
        public readonly uint PetNumber;

        [FieldOffset(-0x18 + 0x164)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5)]
        public readonly uint[] PosStats;

        [FieldOffset(-0x18 + 0x64)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly uint[] Power;

        [FieldOffset(-0x18 + 0x20C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] PowerCostModifier;

        [FieldOffset(-0x18 + 0x228)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] PowerCostMultiplier;

        [FieldOffset(-0x18 + 0xA0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] PowerRegenFlatModifier;

        [FieldOffset(-0x18 + 0xBC)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] PowerRegenInterruptedFlatModifier;

        [FieldOffset(-0x18 + 0x100)]
        public readonly uint RangeAttackTime;

        [FieldOffset(-0x18 + 0x1F8)]
        public readonly uint RangedAttackPower;

        [FieldOffset(-0x18 + 0x1FC)]
        public readonly float RangedAttackPowerMods;

        [FieldOffset(-0x18 + 0x200)]
        public readonly float RangedAttackPowerMultiplier;

        [FieldOffset(-0x18 + 0x1C4)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] ResistanceBuffModsNegative;

        [FieldOffset(-0x18 + 0x1A8)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] ResistanceBuffModsPositive;

        [FieldOffset(-0x18 + 0x18C)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x7)]
        public readonly float[] Resistances;

        [FieldOffset(-0x18 + 0x150)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x5)]
        public readonly uint[] Stats;

        [FieldOffset(-0x18 + 0x20)]
        public readonly ulong Summon;

        [FieldOffset(-0x18 + 0x38)]
        public readonly ulong SummonedBy;

        [FieldOffset(-0x18 + 0x48)]
        public readonly ulong Target;

        [FieldOffset(-0x18 + 0xE0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x3)]
        public readonly uint[] VirtualItemSlotID;
    }
}