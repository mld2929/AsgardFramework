namespace AsgardFramework.WoWAPI.Objects
{
    public class ObjectData
    {
        internal readonly int Base;
        public Common Common { get; internal set; }
        public Position Position { get; internal set; }
        public Object Object { get; internal set; }
        internal ObjectData(int baseAddress, Common common, Position position, Object obj) {
            Base = baseAddress;
            Common = common;
            Position = position;
            Object = obj;
        }
    }
}
