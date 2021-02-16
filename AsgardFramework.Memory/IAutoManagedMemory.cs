namespace AsgardFramework.Memory
{
    public interface IAutoManagedMemory : IMemory
    {
        int Size { get; }
        int Start { get; }
    }
}
