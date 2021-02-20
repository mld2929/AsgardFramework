namespace AsgardFramework.Memory
{
    public interface IGlobalMemory : IMemory
    {
        #region Methods

        IAutoManagedMemory Allocate(int size);

        IAutoManagedSharedBuffer AllocateAutoScalingShared(int minSize);

        IAutoManagedSharedBuffer AllocateShared(int size);

        #endregion Methods
    }
}