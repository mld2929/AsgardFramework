using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IInjectedFunctions
    {
        Task SwitchAntiAFK(bool enabled);

    }
}
