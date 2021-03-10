using System.Collections.Generic;
using System.Text;

namespace AsgardFramework.Memory
{
    public enum WideFieldsWriteType
    {
        ByVal,
        Pointer
    }

    public interface IAutoScalingSharedBuffer : IAutoManagedSharedBuffer
    {
        #region Methods

        IAutoManagedMemory Write<T>(T data) where T : new();

        IAutoManagedMemory Write<T>(T[] data) where T : new();

        IAutoManagedMemory WriteString(string value, Encoding encoding);

        IAutoManagedMemory WriteStruct(IEnumerable<object> data, WideFieldsWriteType forWideFields = WideFieldsWriteType.ByVal, Encoding forStrings = default, int padding = 1);

        #endregion Methods
    }
}