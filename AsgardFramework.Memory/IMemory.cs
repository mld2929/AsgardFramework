namespace AsgardFramework.Memory
{
    public interface IMemory
    {
        byte[] Read(int offset, int count);
        void Write(int offset, byte[] data);
        T Read<T>(int offset) where T : new();
        void Write<T>(int offset, T data) where T : new();
    }
}
