namespace AsgardFramework.Memory
{
    public interface IGlobalMemory : IMemory
    {
        IAutoManagedMemory Allocate(int size);
    }
}
