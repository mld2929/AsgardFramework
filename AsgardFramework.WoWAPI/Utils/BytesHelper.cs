using System;

namespace AsgardFramework.WoWAPI.Utils
{
    internal static class BytesHelper
    {
        internal static int ToInt32(this byte[] bytes) {
            return BitConverter.ToInt32(bytes);
        }

        internal static ulong ToUInt64(this byte[] bytes) {
            return BitConverter.ToUInt64(bytes);
        }

        internal static float ToFloat(this byte[] bytes) {
            return BitConverter.ToSingle(bytes);
        }

        internal static byte[] ToBytes(this int value) {
            return BitConverter.GetBytes(value);
        }

        internal static byte[] ToBytes(this bool value) {
            return BitConverter.GetBytes(value);
        }

        // todo: test
        internal static int[] ToArrayOfInt32(this byte[] bytes) {
            var size = bytes.Length / 4;
            var result = new int[size];
            for (var i = 0; i < size - 1; i++) {
                result[i] = bytes[(i * 4)..((i + 1) * 4)].ToInt32();
            }
            return result;
        }

        internal static float[] ToArrayOfFloat(this byte[] bytes) {
            var size = bytes.Length / 4;
            var result = new float[size];
            for (var i = 0; i < size - 1; i++) {
                result[i] = bytes[(i * 4)..((i + 1) * 4)].ToFloat();
            }
            return result;
        }
    }
}
