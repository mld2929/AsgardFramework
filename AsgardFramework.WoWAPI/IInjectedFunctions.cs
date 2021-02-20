using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IInjectedFunctions
    {
        #region Methods

        Task SwitchAntiAFKAsync(bool enabled);

        #endregion Methods
    }
}