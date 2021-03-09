using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IInjectedFunctions
    {
        #region Properties

        bool AntiAFK { set; }

        #endregion Properties

        #region Methods

        void DisableWarden();

        Task StartExecuteScriptAtEachFrameAsync(string luaScript);

        void Teleport(int playerBase, float x, float y, float z);

        #endregion Methods
    }
}