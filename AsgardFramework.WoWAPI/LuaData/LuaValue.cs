using System;
using System.Collections.Generic;
using System.Linq;

namespace AsgardFramework.WoWAPI.LuaData
{
    public static class LuaValueHelper
    {
        #region Methods

        public static bool ToBool(this string data) {
            return data == "1"; // 1nil -> "1" : string.Empty
        }

        public static double ToDouble(this string data) {
            double.TryParse(data, out var result);

            return result;
        }

        public static T ToEnum<T>(this string data) where T : Enum {
            if (!Enum.TryParse(typeof(T), data, out var result))
                throw new InvalidOperationException($"Can't parse \"{data}\" as {typeof(T)}");

            return (T)result;
        }

        public static int ToInt(this string data) {
            int.TryParse(data, out var result);

            return result;
        }

        #endregion Methods
    }

    public sealed class LuaBoolean : LuaValue<bool>
    {
        #region Methods

        protected override bool ParseData(string[] data) {
            return data[0]
                .ToBool();
        }

        #endregion Methods
    }

    public sealed class LuaEnum<T> : LuaValue<T> where T : Enum
    {
        #region Methods

        protected override T ParseData(string[] data) {
            return data[0]
                .ToEnum<T>();
        }

        #endregion Methods
    }

    public sealed class LuaNumber : LuaValue<double>
    {
        #region Methods

        protected override double ParseData(string[] data) {
            return data[0]
                .ToDouble();
        }

        #endregion Methods
    }

    public sealed class LuaString : LuaValue<string>
    {
        #region Methods

        protected override string ParseData(string[] data) {
            return data[0];
        }

        #endregion Methods
    }

    public sealed class LuaStringList : LuaValue<List<string>>
    {
        #region Methods

        protected override List<string> ParseData(string[] data) {
            return data.ToList();
        }

        #endregion Methods
    }

    public abstract class LuaValue
    {
        #region Methods

        public abstract LuaValue Parse(string[] data);

        #endregion Methods
    }

    public abstract class LuaValue<T> : LuaValue
    {
        #region Fields

        private T value;

        #endregion Fields

        #region Methods

        public static implicit operator T(LuaValue<T> value) {
            return value.value;
        }

        public sealed override LuaValue Parse(string[] data) {
            value = ParseData(data);

            return this;
        }

        protected abstract T ParseData(string[] data);

        #endregion Methods
    }
}