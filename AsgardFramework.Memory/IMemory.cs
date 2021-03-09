using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AsgardFramework.Memory
{
    public interface IMemory : IDisposable
    {
        #region Properties

        bool Disposed { get; }

        #endregion Properties

        #region Events

        event EventHandler Disposing;

        #endregion Events

        #region Methods

        SafeHandle GetHandle();

        byte Read(int offset);

        byte[] Read(int offset, int count);

        T Read<T>(int offset) where T : new();

        T[] Read<T>(int offset, int count) where T : new();

        string ReadNullTerminatedString(int offset, Encoding encoding);

        void Write(int offset, byte value);

        void Write(int offset, byte[] data);

        void Write<T>(int offset, T data) where T : new();

        void Write<T>(int offset, T[] data) where T : new();

        void WriteNullTerminatedString(int offset, string data, Encoding encoding);

        #endregion Methods
    }
}