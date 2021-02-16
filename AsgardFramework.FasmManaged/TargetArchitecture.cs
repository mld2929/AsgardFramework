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
        internal static string AsString(this TargetArchitecture architecture) {
            switch (architecture) {
                case TargetArchitecture.x86_64:
                    return string.Empty;
                case TargetArchitecture.x86:
                    return "use32\n";
                case TargetArchitecture.x64:
                    return "use64\n";
                default:
                    throw new ArgumentOutOfRangeException(nameof(architecture));
            }
        }
    }
}
