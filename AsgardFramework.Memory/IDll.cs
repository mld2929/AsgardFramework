using System;
using System.Text;
using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public delegate int RemoteFunction(params object[] args);

    public interface IDll : IDisposable
    {
        #region Properties

        string Name { get; }

        #endregion Properties

        #region Indexers

        RemoteFunction this[string name, bool isStd, Encoding encoding = default] { get; }

        Task<int> this[string name, bool isStd, Encoding encoding = default, params object[] args] { get; }

        #endregion Indexers
    }
}