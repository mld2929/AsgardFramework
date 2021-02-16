using System.Collections.Generic;

namespace AsgardFramework.WoWAPI
{
    public interface IObjectManager
    {
        IEnumerable<Objects.ObjectData> GetObjects(bool setAllFields);

        Objects.ObjectData Player { get; }

        Objects.ObjectData GetObjectByGuid(ulong guid);
    }
}
