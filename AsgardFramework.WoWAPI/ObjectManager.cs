using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Object = AsgardFramework.WoWAPI.Objects.Object;

namespace AsgardFramework.WoWAPI
{
    internal class ObjectManager : IObjectManager
    {
        private const int c_staticClientConnection = 0x00C79CE0;
        private const int c_objManagerOffset = 0x2ED0;
        private const int c_firstObjOffset = 0xAC;
        private const int c_playerGuidOffset = 0xC0;
        private readonly IFunctions m_functions;
        private readonly IGlobalMemory m_memory;
        private readonly int m_objListStart;
        private readonly int m_pPlayerGuid;

        private ulong m_playerGuid => m_memory.Read(m_pPlayerGuid, 8).ToUInt64();
        public async Task<ObjectData> GetPlayer() => await GetObjectByGuid(m_playerGuid);

        public async Task<ObjectData> GetObjectByGuid(ulong guid) => (await GetObjects(true)).FirstOrDefault(o => o.Object.Guid == guid);
        public async Task<IEnumerable<ObjectData>> GetObjects(bool setAllFields)
        {
            var objects = new ObjectsEnumerable(m_memory, m_objListStart);
            var result = new List<ObjectData>();
            foreach (var common in objects)
            {
                Object obj = null;
                switch (common.Type)
                {
                    case ObjectType.Item:
                        obj = new Item();
                        break;
                    case ObjectType.Container:
                        obj = new Container();
                        break;
                    case ObjectType.Unit:
                        obj = new Unit();
                        break;
                    case ObjectType.Player:
                        obj = new Player();
                        break;
                    case ObjectType.GameObject:
                        obj = new GameObject();
                        break;
                    case ObjectType.DynamicObject:
                        obj = new DynamicObject();
                        break;
                    case ObjectType.Corpse:
                        obj = new Corpse();
                        break;
                    default:
                        continue;
                }
                var data = new ObjectData(common, new Position(), obj);
                result.Add(data);
                if (setAllFields)
                {
                    await m_functions.UpdatePosition(data);
                }
            }
            return result;
        }

        internal ObjectManager(IGlobalMemory memory, IFunctions functions)
        {
            m_functions = functions;
            m_memory = memory;

            var pObjManager = m_memory.Read(c_staticClientConnection, 4).ToInt32() + c_objManagerOffset;
            var objManager = m_memory.Read(pObjManager, 4).ToInt32();
            m_objListStart = m_memory.Read(objManager + c_firstObjOffset, 4).ToInt32();
            m_pPlayerGuid = objManager + c_playerGuidOffset;
        }
    }
}
