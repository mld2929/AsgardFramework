using System.Collections.Generic;

using AsgardFramework.Memory.Implementation;

namespace AsgardFramework.Memory
{
    internal interface ICoreDll : IDll
    {
        #region Methods

        InterprocessManualResetEventSlim InitializeInteraction(IAutoManagedMemory queue);

        void RegisterFunction(string name, FunctionCallType type, int address, int argumentsCount);

        void RegisterFunctions(IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> functions);

        #endregion Methods
    }
}