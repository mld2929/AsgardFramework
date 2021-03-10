using System.Collections.Generic;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.LuaData;

namespace AsgardFramework.WoWAPI
{
    public interface ILuaScriptExecutor
    {
        #region Methods

        Task RunScriptAsync(string script);

        Task<T> RunScriptAsync<T>(string script, int returnCount = 10) where T : LuaValue, new();

        Task<IReadOnlyList<string>> RunScriptAsync(string script, IEnumerable<string> returnVariables);

        #endregion Methods
    }
}