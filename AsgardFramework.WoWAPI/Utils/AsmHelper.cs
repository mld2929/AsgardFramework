namespace AsgardFramework.WoWAPI.Utils
{
    internal static class AsmHelper
    {
        #region Methods

        /// <summary>
        ///     Safely calls address, writing it in eax register
        /// </summary>
        /// <remarks>
        ///     Don't know why, but <c>call dword [0xDEADBEEF]</c> doesn't work correctly
        /// </remarks>
        internal static string CallViaEax(this int value) {
            return $"mov eax, {value}\ncall eax";
        }

        internal static string Push(this ulong value) {
            return unchecked($"push 0x{(int)(value >> 32):X} 0x{(int)value:X}");
        }

        #endregion Methods
    }
}