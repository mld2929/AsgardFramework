using System;
using System.Text;
using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public delegate Task<int> RemoteAsyncFunction(params object[] args);

    public delegate int RemoteFunction(params object[] args);

    public interface IDll : IDisposable
    {
        #region Properties

        string Name { get; }

        #endregion Properties

        #region Indexers

        RemoteFunction this[string name, bool isStd, Encoding encoding = default] { get; }

        RemoteAsyncFunction this[bool isStd, string name, Encoding encoding = default] { get; }

        #endregion Indexers
    }
}