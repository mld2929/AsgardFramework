namespace AsgardFramework.WoWAPI.Objects
{
    public class ObjectData
    {
        #region Fields

        public readonly int Base;

        #endregion Fields

        #region Constructors

        internal ObjectData(int baseAddress, Common common, Position position, Object obj) {
            Base = baseAddress;
            Common = common;
            Position = position;
            Object = obj;
        }

        #endregion Constructors

        #region Properties

        public Common Common { get; }
        public string Name { get; internal set; }
        public Object Object { get; internal set; }
        public Position Position { get; internal set; }

        #endregion Properties
    }
}