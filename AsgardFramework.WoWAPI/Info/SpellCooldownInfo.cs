using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 12)]
    public class SpellCooldownInfo
    {
        /// <summary>
        ///     <see langword="true" /> if a Cooldown UI element should be used to display the cooldown, otherwise
        ///     <see langword="false" />.
        /// </summary>
        /// <remarks>
        ///     Does not always correlate with whether the spell is ready.
        /// </remarks>
        [FieldOffset(8)]
        [MarshalAs(UnmanagedType.U4)]
        public readonly bool CooldownVisibleAtUI;

        /// <summary>
        ///     The length of the cooldown, or 0 if the spell is ready
        /// </summary>
        [FieldOffset(4)]
        public readonly float Duration;

        /// <summary>
        ///     Time in seconds (with millisecond precision) at the moment the cooldown began, or 0 if the spell is ready
        /// </summary>
        [FieldOffset(0)]
        public readonly float Start;
    }
}