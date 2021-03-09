using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public interface IGlobalMemory : IMemory
    {
        #region Methods

        IAutoManagedMemory Allocate(int size);

        IAutoScalingSharedBuffer AllocateAutoScalingShared(int minSize);

        IAutoManagedSharedBuffer AllocateShared(int size);

        IMainThreadExecutor GetMainThreadExecutor();

        IDll LoadDll(string path);

        Task<IDll> LoadDllAsync(string path);

        #endregion Methods
    }
}