namespace AsgardFramework.WoWAPI.Objects
{
    public class ObjectData
    {
        public readonly Common Common;
        public readonly Position Position;
        public readonly Object Object;
        internal ObjectData(Common common, Position position, Object obj)
        {
            Common = common;
            Position = position;
            Object = obj;
        }
    }
}
