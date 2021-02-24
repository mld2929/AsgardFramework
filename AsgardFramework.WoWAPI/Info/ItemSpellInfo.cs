using System.Runtime.InteropServices;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal readonly struct ItemSpellInfoRaw
    {
        public readonly int Name;

        public readonly int RankOrSecondaryText;
    }

    public class ItemSpellInfo
    {
        #region Fields

        /// <summary>
        ///     Name of the spell
        /// </summary>
        public readonly string Name;

        /// <summary>
        ///     Secondary text associated with the spell
        /// </summary>
        /// <remarks>
        ///     Often a rank, e.g. <c>"Rank 7"</c>; or the <see cref="string.Empty" /> if not applicable
        /// </remarks>
        public readonly string RankOrSecondaryText;

        #endregion Fields

        #region Constructors

        internal ItemSpellInfo(ItemSpellInfoRaw raw, IGlobalMemory memory) {
            Name = memory.ReadNullTerminatedString(raw.Name, Encoding.UTF8);
            RankOrSecondaryText = memory.ReadNullTerminatedString(raw.RankOrSecondaryText, Encoding.UTF8);
        }

        #endregion Constructors
    }
}