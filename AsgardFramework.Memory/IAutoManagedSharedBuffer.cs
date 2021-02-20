using System;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedSharedBuffer : IDisposable
    {
        #region Properties

        int Size { get; }

        #endregion Properties

        #region Methods

        bool TryReserve(int size, out IAutoManagedMemory reserved);

        #endregion Methods
    }
}