using System;

namespace AsgardFramework.WoWAPI.LuaData
{
    [Flags]
    public enum BagType
    {
        Default = 0,
        Quiver = 0x0001,
        AmmoPouch = 0x2,
        SoulBag = 0x4,
        LeatherworkingBag = 0x8,
        InscriptionBag = 0x10,
        HerbBag = 0x20,
        EnchantingBag = 0x40,
        EngineeringBag = 0x80,
        Keyring = 0x100,
        GemBag = 0x200,
        MiningBag = 0x400,
        Unused = 0x800,
        VanityPets = 0x1000,
        TackleBox = 0x100000
    }

    public sealed class LuaBagType : LuaValue<BagType>
    {
        #region Methods

        protected override BagType ParseData(string[] data) {
            return (BagType)int.Parse(data[0]);
        }

        #endregion Methods
    }
}