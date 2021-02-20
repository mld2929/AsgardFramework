namespace AsgardFramework.Memory
{
    public interface IMemory
    {
        #region Methods

        byte[] Read(int offset, int count);

        T Read<T>(int offset) where T : new();

        void Write(int offset, byte[] data);

        void Write<T>(int offset, T data) where T : new();

        #endregion Methods
    }
}