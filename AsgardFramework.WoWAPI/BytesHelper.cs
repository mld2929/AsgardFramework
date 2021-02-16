using System;

namespace AsgardFramework.WoWAPI
{
    internal static class BytesHelper
    {
        internal static int ToInt32(this byte[] bytes) => BitConverter.ToInt32(bytes);

        internal static byte[] ToBytes(this int value) => BitConverter.GetBytes(value);

        internal static byte[] ToBytes(this bool value) => BitConverter.GetBytes(value);
    }
}
