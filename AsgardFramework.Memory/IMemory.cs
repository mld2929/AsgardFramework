namespace AsgardFramework.Memory
{
    public interface IMemory
    {
        byte[] Read(int offset, int count);
        void Write(int offset, byte[] data);

        // todo implement something better
        T Read<T>(int offset) where T : class, new();
        void Write<T>(int offset, T data) where T : class, new();
    }
}
