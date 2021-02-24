using System.Runtime.InteropServices;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Info
{
    public class UnitClassInfo
    {
        /// <summary>
        ///     The localized name of the unit's class, or the unit's name if the unit is an NPC
        /// </summary>
        public readonly string LocalizedName;

        /// <summary>
        ///     A non-localized token representing the class
        /// </summary>
        public readonly string Name;

        internal UnitClassInfo(UnitClassInfoRaw raw, IGlobalMemory memory) {
            LocalizedName = memory.ReadNullTerminatedString(raw.LocalizedName, Encoding.UTF8);
            Name = memory.ReadNullTerminatedString(raw.Name, Encoding.UTF8);
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct UnitClassInfoRaw
    {
        /// <summary>
        ///     The localized name of the unit's class, or the unit's name if the unit is an NPC
        /// </summary>
        public readonly int LocalizedName;

        /// <summary>
        ///     A non-localized token representing the class
        /// </summary>
        public readonly int Name;
    }
}