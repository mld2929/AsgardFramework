using System.Runtime.InteropServices;

namespace AsgardFramework.Memory
{
    /// <summary>
    ///     For dynamic-sized types; inherited classes must append <see cref="StructLayoutAttribute" /> with
    ///     <see cref="LayoutKind.Sequential" /> (or <see cref="LayoutKind.Explicit" />, but how?)
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public abstract class SizePrefixed
    {
        internal readonly int Size;
    }
}