using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class ObjectManager : IObjectManager
    {
        #region Constructors

        internal ObjectManager(IGlobalMemory memory, IGameFunctions functions, IAggregatedFunctions aggregatedFunctions) {
            m_functions = functions;
            m_aggregatedFunctions = aggregatedFunctions;
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

        private readonly IAggregatedFunctions m_aggregatedFunctions;

        private readonly IGameFunctions m_functions;

        private readonly IGlobalMemory m_memory;

        private readonly int m_objListStart;

        private readonly int m_pPlayerGuid;

        #endregion Fields

        #region Methods

        public Item ContainerAsItem(Common container) {
            return m_memory.Read<Item>(container.Fields);
        }

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

        // todo: rewrite
        public async Task<IEnumerable<ObjectData>> GetObjectsAsync(bool setAllFields) {
            var data = getRawEnumerable()
                .ToList();

            if (!setAllFields)
                return data;

            var playerGuid = m_playerGuid;

            var ids = data.Select(obj => obj.Base);

            var objects = data.Select(d => readObject(d.Common, playerGuid))
                              .ToList();

            var positions = (await m_aggregatedFunctions.GetPositionsAsync(ids)
                                                        .ConfigureAwait(false)).ToList();

            var names = (await m_aggregatedFunctions.GetNamesAsync(ids)
                                                    .ConfigureAwait(false)).ToList();

            for (var i = 0; i < data.Count; i++) {
                var current = data[i];
                current.Object = objects[i];
                current.Position = positions[i];
                current.Name = names[i];
            }

            return data;
        }

        public Task<ObjectData> GetPlayerAsync() {
            return GetObjectByGuidAsync(m_playerGuid);
        }

        private IEnumerable<ObjectData> getRawEnumerable() {
            return new ObjectsEnumerable(m_memory, m_objListStart);
        }

        private Container readContainer(Common data) {
            var result = m_memory.Read<Container>(data.Fields);
            var start = data.Fields + Marshal.SizeOf<Container>() - 8;

            result.Items = m_memory.Read<ulong>(start, result.Slots);

            return result;
        }

        private Object readObject(Common commonData, ulong playerGuid) {
            return commonData?.Type switch {
                ObjectType.Item => m_memory.Read<Item>(commonData.Fields),
                ObjectType.Container => readContainer(commonData),
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