using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IInjectedFunctions
    {
        #region Methods

        Task StartExecuteScriptAtEachFrameAsync(string luaScript);

        Task SwitchAntiAFKAsync(bool enabled);

        #endregion Methods
    }
}