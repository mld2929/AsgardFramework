using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public delegate int RemoteMainThreadFunction(params object[] arguments);

    /// <summary>
    ///     Allows to call functions in main thread
    /// </summary>
    public interface IMainThreadExecutor
    {
        #region Indexers

        Task<int> this[string functionName, params object[] arguments] { get; }

        RemoteMainThreadFunction this[string functionName] { get; }

        #endregion Indexers

        #region Methods

        RemoteMainThreadFunction RegisterFunction(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount);

        IReadOnlyList<RemoteMainThreadFunction> RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions);

        #endregion Methods
    }
}