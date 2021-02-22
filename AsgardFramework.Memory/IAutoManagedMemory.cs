using System;
using System.Collections.Generic;

namespace AsgardFramework.Memory
{
    public interface IAutoManagedMemory : IMemory
    {
        #region Methods

        public static int operator+(IAutoManagedMemory memory, int offset) {
            return memory.Start + offset;
        }

        #endregion Methods

        #region Properties

        int Size { get; }

        int Start { get; }

        #endregion Properties

        #region Indexers

        IEnumerable<byte> this[Range range] { get; set; }

        byte this[Index index] { get; set; }

        #endregion Indexers
    }
}