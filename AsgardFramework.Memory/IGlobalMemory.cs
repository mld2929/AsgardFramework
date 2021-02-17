namespace AsgardFramework.Memory
{
    public interface IGlobalMemory : IMemory
    {
        IAutoManagedMemory Allocate(int size);

        IAutoManagedSharedBuffer AllocateShared(int size);

        IAutoManagedSharedBuffer AllocateAutoScalingShared(int minSize);
    }
}
