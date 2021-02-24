using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IInjectedFunctions
    {
        #region Methods

        Task StartExecuteScriptAtEachFrameAsync(string luaScript);

        bool AntiAFK { set; }

        #endregion Methods
    }
}