using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AsgardFramework.Memory
{
    public delegate Task<int> RemoteMainThreadAsyncFunction(params object[] arguments);

    public delegate Task<int[]> RemoteMainThreadAsyncFunctionsChain(params object[][] arguments);

    public delegate int RemoteMainThreadFunction(params object[] arguments);

    public enum FunctionCallType
    {
        /// <summary>
        ///     Caller cleans up stack, arguments pushed to stack
        /// </summary>
        Cdecl,

        /// <summary>
        ///     Callee cleans up stack, arguments pushed to stack
        /// </summary>
        Stdcall,

        /// <summary>
        ///     Caller cleans up stack, first argument (<see langword="this" />) moved to ecx, other arguments pushed to stack
        /// </summary>
        Thiscall,

        /// <summary>
        ///     Caller cleans up stack, first argument (<see langword="this" />) moved to ecx, other arguments pushed to stack;
        ///     While registration function address is index of function
        /// </summary>
        Virtualcall
    }

    /// <summary>
    ///     Allows to call functions in main thread
    /// </summary>
    public interface IMainThreadExecutor
    {
        #region Indexers

        /// <summary>
        ///     Gives delegate for async function call
        /// </summary>
        RemoteMainThreadAsyncFunction this[string functionName] { get; }

        /// <summary>
        ///     Gives delegate for sync function call
        /// </summary>
        RemoteMainThreadFunction this[string functionName, int timeout = Timeout.Infinite] { get; }

        /// <summary>
        ///     Gives delegate for async sequiential functions call
        /// </summary>
        RemoteMainThreadAsyncFunctionsChain this[IEnumerable<string> functions] { get; }

        #endregion Indexers

        #region Methods

        /// <summary>
        ///     Registers function for usage by name
        /// </summary>
        /// <param name="functionName">Name of function (any)</param>
        /// <param name="functionType">Call type</param>
        /// <param name="functionAddress">Address or index (for <see cref="FunctionCallType.Virtualcall" />) of function</param>
        /// <param name="argumentsCount">Count of arguments, can be 0</param>
        void RegisterFunction(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount);

        void RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions);

        #endregion Methods
    }
}