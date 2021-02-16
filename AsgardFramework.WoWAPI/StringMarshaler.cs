using System;
using System.Runtime.InteropServices;

namespace AsgardFramework.WoWAPI
{
    internal class StringMarshaler : ICustomMarshaler
    {
        public void CleanUpManagedData(object ManagedObj) => throw new NotImplementedException();
        public void CleanUpNativeData(IntPtr pNativeData) => throw new NotImplementedException();
        public int GetNativeDataSize() => throw new NotImplementedException();
        public IntPtr MarshalManagedToNative(object ManagedObj) => throw new NotImplementedException();
        public object MarshalNativeToManaged(IntPtr pNativeData) => throw new NotImplementedException();
    }
}
