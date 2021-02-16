using System;

namespace AsgardFramework.WoWAPI
{
    internal static class BytesHelper
    {
        internal static int ToInt32(this byte[] bytes) {
            return BitConverter.ToInt32(bytes);
        }

        internal static ulong ToUInt64(this byte[] bytes) {
            return BitConverter.ToUInt64(bytes);
        }

        internal static byte[] ToBytes(this int value) {
            return BitConverter.GetBytes(value);
        }

        internal static byte[] ToBytes(this bool value) {
            return BitConverter.GetBytes(value);
        }
    }
}
