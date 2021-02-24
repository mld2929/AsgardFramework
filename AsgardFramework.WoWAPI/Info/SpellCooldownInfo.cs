using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI.Info
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SpellCooldownInfo
    {
        /// <summary>
        ///     Time in seconds (with millisecond precision) at the moment the cooldown began, or 0 if the spell is ready
        /// </summary>
        public readonly double Start;

        /// <summary>
        ///     The length of the cooldown, or 0 if the spell is ready
        /// </summary>
        public readonly double Duration;
        /// <summary>
        ///     <see langword="true" /> if a Cooldown UI element should be used to display the cooldown, otherwise
        ///     <see langword="false" />.
        /// </summary>
        /// <remarks>
        ///     Does not always correlate with whether the spell is ready.
        /// </remarks>
        [MarshalAs(UnmanagedType.Bool)]
        public readonly bool CooldownVisibleAtUI;

    }
}