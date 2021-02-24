using System.Runtime.InteropServices;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct UnitFactionGroupInfoRaw
    {
        public readonly int Name;
        public readonly int LocalizedName;
    }

    public class UnitFactionGroupInfo
    {
        #region Fields

        /// <summary>
        ///     Localized name of the faction
        /// </summary>
        public readonly string LocalizedName;

        /// <summary>
        ///     Non-localized (English) faction name of the faction (<c>"Horde"</c> or <c>"Alliance"</c>)
        /// </summary>
        public readonly string Name;

        #endregion Fields

        #region Constructors

        internal UnitFactionGroupInfo(UnitFactionGroupInfoRaw raw, IGlobalMemory memory) {
            Name = memory.ReadNullTerminatedString(raw.Name, Encoding.UTF8);
            LocalizedName = memory.ReadNullTerminatedString(raw.LocalizedName, Encoding.UTF8);
        }

        #endregion Constructors
    }
}