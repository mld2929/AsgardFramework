using System;
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

        byte[] Read(int offset, int count);

        T Read<T>(int offset) where T : new();

        string ReadString(int offset, Encoding encoding);

        void Write(int offset, byte[] data);

        void Write<T>(int offset, T data) where T : new();

        void WriteString(int offset, string data, Encoding encoding);

        #endregion Methods
    }
}