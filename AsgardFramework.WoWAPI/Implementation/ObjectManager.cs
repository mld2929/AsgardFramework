using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class ObjectManager : IObjectManager
    {
        #region Constructors

        internal ObjectManager(IGlobalMemory memory, IGameFunctions functions) {
            m_functions = functions;
            m_memory = memory;

            var pObjManager = m_memory.Read<int>(c_staticClientConnection) + c_objManagerOffset;
            var objManager = m_memory.Read<int>(pObjManager);
            m_objListStart = m_memory.Read<int>(objManager + c_firstObjOffset);
            m_pPlayerGuid = objManager + c_playerGuidOffset;
        }

        #endregion Constructors

        #region Properties

        private ulong m_playerGuid => m_memory.Read<ulong>(m_pPlayerGuid);

        #endregion Properties

        #region Fields

        private const int c_firstObjOffset = 0xAC;
        private const int c_objManagerOffset = 0x2ED0;
        private const int c_playerGuidOffset = 0xC0;
        private const int c_staticClientConnection = 0x00C79CE0;
        private readonly IGameFunctions m_functions;
        private readonly IGlobalMemory m_memory;
        private readonly int m_objListStart;
        private readonly int m_pPlayerGuid;

        #endregion Fields

        #region Methods

        public async Task<ObjectData> GetObjectByGuidAsync(ulong guid) {
            return (await GetObjectsAsync(true)
                        .ConfigureAwait(false)).FirstOrDefault(o => o.Object.Guid == guid);
        }

        public async Task<IEnumerable<ObjectData>> GetObjectsAsync(bool setAllFields) {
            var objects = new ObjectsEnumerable(m_memory, m_objListStart).ToList();

            if (!setAllFields)
                return objects;

            var playerGuid = m_playerGuid;

            foreach (var obj in objects) {
                switch (obj.Common.Type) {
                    case ObjectType.Item:
                        obj.Object = m_memory.Read<Item>(obj.Common.Fields);

                        break;

                    case ObjectType.Container:
                        obj.Object = m_memory.Read<Container>(obj.Common.Fields);

                        break;

                    case ObjectType.Unit:
                        obj.Object = m_memory.Read<Unit>(obj.Common.Fields);

                        break;

                    case ObjectType.Player:
                        obj.Object = m_memory.Read<Unit>(obj.Common.Fields);

                        if (obj.Object.Guid == playerGuid)
                            obj.Object = m_memory.Read<Player>(obj.Common.Fields);

                        break;

                    case ObjectType.GameObject:
                        obj.Object = m_memory.Read<GameObject>(obj.Common.Fields);

                        break;

                    case ObjectType.DynamicObject:
                        obj.Object = m_memory.Read<DynamicObject>(obj.Common.Fields);

                        break;

                    case ObjectType.Corpse:
                        obj.Object = m_memory.Read<Corpse>(obj.Common.Fields);

                        break;

                    default:
                        continue;
                }

                obj.Position = await m_functions.GetPositionAsync(obj.Base)
                                                .ConfigureAwait(false);
            }

            return objects;
        }

        public async Task<ObjectData> GetPlayerAsync() {
            var playerGuid = m_playerGuid;

            var player = (await GetObjectsAsync(false)
                              .ConfigureAwait(false)).First(obj => obj.Common.Guid == playerGuid);

            player.Object = m_memory.Read<Player>(player.Base);

            player.Position = await m_functions.GetPositionAsync(player.Base)
                                               .ConfigureAwait(false);

            return player;
        }

        #endregion Methods
    }
}