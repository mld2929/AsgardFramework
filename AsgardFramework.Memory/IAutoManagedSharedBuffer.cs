using System;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedSharedBuffer : IDisposable
    {
        #region Properties

        int Size { get; }

        #endregion Properties

        #region Methods

        IAutoManagedMemory Reserve(int size);

        bool TryReserve(int size, out IAutoManagedMemory reserved);

        #endregion Methods
    }
}