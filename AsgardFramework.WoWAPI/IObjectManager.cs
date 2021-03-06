using System.Collections.Generic;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    public interface IObjectManager
    {
        #region Methods

        Task<ObjectData> GetObjectByGuidAsync(ulong guid);

        Task<IEnumerable<ObjectData>> GetObjectsAsync(bool setAllFields);

        Task<ObjectData> GetPlayerAsync();
        Item ContainerAsItem(Common container);

        #endregion Methods
    }
}