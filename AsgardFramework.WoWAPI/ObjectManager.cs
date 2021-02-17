﻿using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ObjectData> GetPlayer() {
            return await GetObjectByGuid(m_playerGuid);
        }

        public async Task<ObjectData> GetObjectByGuid(ulong guid) {
            return (await GetObjects(true)).FirstOrDefault(o => o.Object.Guid == guid);
        }

        public async Task<IEnumerable<ObjectData>> GetObjects(bool setAllFields) {
            var objects = new ObjectsEnumerable(m_memory, m_objListStart).ToList();
            if (setAllFields) {
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

                            if (obj.Object.Guid == playerGuid) {
                                obj.Object = m_memory.Read<Player>(obj.Common.Fields);
                            }

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

                    await m_functions.UpdatePosition(obj);
                }
            }

            return objects;
        }

        internal ObjectManager(IGlobalMemory memory, IFunctions functions) {
            m_functions = functions;
            m_memory = memory;

            var pObjManager = m_memory.Read(c_staticClientConnection, 4).ToInt32() + c_objManagerOffset;
            var objManager = m_memory.Read(pObjManager, 4).ToInt32();
            m_objListStart = m_memory.Read(objManager + c_firstObjOffset, 4).ToInt32();
            m_pPlayerGuid = objManager + c_playerGuidOffset;
        }
    }
}
