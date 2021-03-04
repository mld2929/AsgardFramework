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
            var obj = getRawEnumerable()
                .FirstOrDefault(o => o.Common.Guid == guid);

            if (obj == null)
                return null;

            obj.Position = await m_functions.GetPositionAsync(obj.Base)
                                            .ConfigureAwait(false);

            obj.Object = readObject(obj.Common, m_playerGuid);

            obj.Name = await m_functions.GetNameAsync(obj.Base)
                                        .ConfigureAwait(false);

            return obj;
        }

        public async Task<IEnumerable<ObjectData>> GetObjectsAsync(bool setAllFields) {
            var objects = getRawEnumerable()
                .ToList();

            if (!setAllFields)
                return objects;

            var playerGuid = m_playerGuid;

            foreach (var obj in objects) {
                obj.Object = readObject(obj.Common, playerGuid);

                obj.Position = await m_functions.GetPositionAsync(obj.Base)
                                                .ConfigureAwait(false);

                obj.Name = await m_functions.GetNameAsync(obj.Base)
                                            .ConfigureAwait(false);
            }

            return objects;
        }

        public Task<ObjectData> GetPlayerAsync() {
            return GetObjectByGuidAsync(m_playerGuid);
        }

        private IEnumerable<ObjectData> getRawEnumerable() {
            return new ObjectsEnumerable(m_memory, m_objListStart);
        }

        private Object readObject(Common commonData, ulong playerGuid) {
            return commonData?.Type switch {
                ObjectType.Item => m_memory.Read<Item>(commonData.Fields),
                ObjectType.Container => m_memory.Read<Container>(commonData.Fields),
                ObjectType.Unit => m_memory.Read<Unit>(commonData.Fields),
                ObjectType.Player => commonData.Guid == playerGuid ? m_memory.Read<Player>(commonData.Fields) : m_memory.Read<Unit>(commonData.Fields),
                ObjectType.GameObject => m_memory.Read<GameObject>(commonData.Fields),
                ObjectType.DynamicObject => m_memory.Read<DynamicObject>(commonData.Fields),
                ObjectType.Corpse => m_memory.Read<Corpse>(commonData.Fields),
                _ => null
            };
        }

        #endregion Methods
    }
}