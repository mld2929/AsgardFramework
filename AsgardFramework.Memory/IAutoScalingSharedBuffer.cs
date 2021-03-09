using System.Text;

namespace AsgardFramework.Memory
{
    public enum WriteType
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

        IAutoManagedMemory WriteStruct(object[] data, WriteType forWideFields, Encoding forStrings, int padding = 1);

        #endregion Methods
    }
}