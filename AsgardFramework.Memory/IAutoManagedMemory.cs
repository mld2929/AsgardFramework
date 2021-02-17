using System;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedMemory : IMemory, IDisposable
    {
        int Size { get; }
        int Start { get; }
    }
}
