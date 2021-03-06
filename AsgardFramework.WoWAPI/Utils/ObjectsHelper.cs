using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI.Utils
{
    public static class ObjectsHelper
    {
        #region Methods

        public static T As<T>(this Object obj) where T : Object {
            return obj as T;
        }

        public static Item ToItem(this Object obj) {
            return (Item)obj;
        }

        public static Player ToPlayer(this Object obj) {
            return (Player)obj;
        }

        #endregion Methods
    }
}