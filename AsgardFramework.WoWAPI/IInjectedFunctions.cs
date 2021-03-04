using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IInjectedFunctions
    {
        #region Methods

        bool AntiAFK { set; }

        void DisableWarden();

        Task StartExecuteScriptAtEachFrameAsync(string luaScript);

        #endregion Methods
    }
}