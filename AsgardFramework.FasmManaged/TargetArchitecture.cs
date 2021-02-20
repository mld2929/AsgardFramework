using System;

namespace AsgardFramework.FasmManaged
{
    public enum TargetArchitecture
    {
        x86_64,
        x86,
        x64
    }

    internal static class TargetArchitectureHelper
    {
        #region Methods

        internal static string AsString(this TargetArchitecture architecture) {
            return architecture switch {
                TargetArchitecture.x86_64 => string.Empty,
                TargetArchitecture.x86 => "use32\n",
                TargetArchitecture.x64 => "use64\n",
                _ => throw new ArgumentOutOfRangeException(nameof(architecture))
            };
        }

        #endregion Methods
    }
}