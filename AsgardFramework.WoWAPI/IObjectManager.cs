using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsgardFramework.WoWAPI
{
    public interface IObjectManager
    {
        Task<IEnumerable<Objects.ObjectData>> GetObjects(bool setAllFields);

        Task<Objects.ObjectData> GetPlayer();

        Task<Objects.ObjectData> GetObjectByGuid(ulong guid);
    }
}
