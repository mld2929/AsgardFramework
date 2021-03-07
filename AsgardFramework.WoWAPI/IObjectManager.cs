using System.Collections.Generic;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    public interface IObjectManager
    {
        #region Methods

        Item ContainerAsItem(Common container);

        Task<ObjectData> GetObjectByGuidAsync(ulong guid);

        Task<IEnumerable<ObjectData>> GetObjectsAsync(bool setAllFields);

        Task<ObjectData> GetPlayerAsync();

        #endregion Methods
    }
}