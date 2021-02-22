using System;
using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal partial class FunctionsAccessor : IInjectedFunctions
    {
        #region Methods

        public Task StartExecuteScriptAtEachFrameAsync(string luaScript) {
            throw new NotImplementedException();
        }

        public Task SwitchAntiAFKAsync(bool enabled) {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}