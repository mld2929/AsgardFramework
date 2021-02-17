using System;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedSharedBuffer : IDisposable
    {
        int Size { get; }
        bool TryReserve(int size, out IAutoManagedMemory reserved);
    }
}
