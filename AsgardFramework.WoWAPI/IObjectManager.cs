using System.Collections.Generic;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    public interface IObjectManager
    {
        #region Methods

        Item ContainerAsItem(Common container);

#nullable enable

        Task<ObjectData?> GetObjectByGuidAsync(ulong guid);

#nullable restore

        Task<IEnumerable<ObjectData>> GetObjectsAsync(bool setAllFields);

        Task<ObjectData> GetPlayerAsync();

        #endregion Methods
    }
}