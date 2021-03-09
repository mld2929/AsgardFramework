using System;
using System.Collections.Generic;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedMemory : IMemory
    {
        #region Properties

        int Size { get; }

        int Start { get; }

        #endregion Properties

        #region Indexers

        IEnumerable<byte> this[Range range] { get; set; }

        byte this[Index index] { get; set; }

        #endregion Indexers

        #region Methods

        public static int operator+(IAutoManagedMemory memory, int offset) {
            return memory.Start + offset;
        }

        void Fill<T>(T value) where T : new();

        #endregion Methods
    }
}