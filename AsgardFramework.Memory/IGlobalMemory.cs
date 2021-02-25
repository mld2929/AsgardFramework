namespace AsgardFramework.Memory
{
    public interface IGlobalMemory : IMemory
    {
        #region Methods

        IAutoManagedMemory Allocate(int size);

        IAutoScalingSharedBuffer AllocateAutoScalingShared(int minSize);

        IAutoManagedSharedBuffer AllocateShared(int size);

        void LoadDll(string path);

        #endregion Methods
    }
}