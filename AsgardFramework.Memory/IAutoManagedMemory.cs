using System;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedMemory : IMemory, IDisposable
    {
        #region Properties

        int Size { get; }
        int Start { get; }

        #endregion Properties
    }
}