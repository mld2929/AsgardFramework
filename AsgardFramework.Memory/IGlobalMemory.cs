using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public interface IGlobalMemory : IMemory
    {
        #region Methods

        IAutoManagedMemory Allocate(int size);

        IAutoScalingSharedBuffer AllocateAutoScalingShared(int minSize);

        IAutoManagedSharedBuffer AllocateShared(int size);

        IDll LoadDll(string fullPath, string dllName);

        Task<IDll> LoadDllAsync(string fullPath, string dllName);

        #endregion Methods
    }
}