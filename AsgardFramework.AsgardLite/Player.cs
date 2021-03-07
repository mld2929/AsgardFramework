using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI;
using AsgardFramework.WoWAPI.LuaData;
using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.AsgardLite
{
    public sealed class Player
    {
        #region Fields

        private readonly IGameWrapper m_game;

        #endregion Fields

        #region Constructors

        public Player(IGameWrapper game) {
            m_game = game;
        }

        #endregion Constructors

        #region Properties

        public Task<IReadOnlyList<Bag>> Bags => getBagsAsync();
        public Task<bool> InCombat => m_game.GameAPIFunctions.IsUnitAffectingCombatAsync(UnitMetaId.Player);
        public Task<IReadOnlyList<Item>> Inventory => getInventoryAsync();
        public Task<bool> IsConnected => m_game.GameAPIFunctions.UnitIsConnectedAsync(UnitMetaId.Player);
        public Task<bool> IsDead => m_game.GameAPIFunctions.UnitIsDeadOrGhostAsync(UnitMetaId.Player);
        public string Name => m_game.PlayerName;
        private Task<ObjectData> m_player => m_game.ObjectManager.GetPlayerAsync();

        #endregion Properties

        #region Methods

        public Task CastSpellAsync(string name, UnitMetaId target = null) {
            return m_game.GameAPIFunctions.RunScriptAsync($"CastSpellByName({name}" + (target != null ? $", {target})" : ")"));
        }

        public Task CastSpellAsync(int id, UnitMetaId target = null) {
            return m_game.GameAPIFunctions.RunScriptAsync($"CastSpellByID({id.ToString(CultureInfo.InvariantCulture)}" + (target != null ? $", {target})" : ")"));
        }

        public async Task<int> GetFreeSlotsFor(BagType type) {
            return (await Bags.ConfigureAwait(false)).Where(b => b.Type.HasFlag(type))
                                                     .Sum(b => b.FreeSlots);
        }

        public Task MoveAsync(Vector3 to) {
            return m_game.GameFunctions.ClickToMoveAsync(to.X, to.Y, to.Z, 0x4, 0, 0.5f);
        }

        public Task MoveAsync(float toX, float toY, float toZ) {
            return m_game.GameFunctions.ClickToMoveAsync(toX, toY, toZ, 0x4, 0, 0.5f);
        }

        public Task StartAttackAsync() {
            return m_game.GameAPIFunctions.AttackTargetAsync();
        }

        public Task StopAttackAsync() {
            return m_game.GameAPIFunctions.RunScriptAsync("StopAttack()");
        }

        public Task StopMovingAsync() {
            return m_game.GameFunctions.ClickToMoveStopAsync();
        }

        public async Task Teleport(float toX, float toY, float toZ) {
            m_game.InjectedFunctions.Teleport((await m_player.ConfigureAwait(false)).Base, toX, toY, toZ);
        }

        private async Task<IReadOnlyList<Bag>> getBagsAsync() {
            var player = (await m_player.ConfigureAwait(false)).Object.As<WoWAPI.Objects.Player>();

            var objects = (await m_game.ObjectManager.GetObjectsAsync(true)
                                       .ConfigureAwait(false)).Where(obj => obj.Common.Type == ObjectType.Item || obj.Common.Type == ObjectType.Container)
                                                              .ToList();

            var containers = objects.Where(obj => obj.Common.Type == ObjectType.Container && player.Inventory.Contains(obj.Common.Guid))
                                    .Select(obj => obj.Object.As<Container>())
                                    .ToList();

            var items = objects.Where(obj => !containers.Contains(obj.Object))
                               .Select(obj => obj.Common.Type switch {
                                   ObjectType.Container => m_game.ObjectManager.ContainerAsItem(obj.Common),
                                   ObjectType.Item => obj.Object.As<Item>(),
                                   _ => throw new ArgumentOutOfRangeException(nameof(obj.Common.Type))
                               })
                               .ToList();

            IReadOnlyList<Item> joinAndRemoveFromList(ulong[] guids) {
                var result = items.Join(guids, item => item.Guid, guid => guid, (item, _) => item)
                                  .ToList();

                items = items.Except(result)
                             .ToList();

                return result;
            }

            async Task<Bag> createBag(Container c, int i) {
                return new Bag(joinAndRemoveFromList(c.Items), c.Slots, (await m_game.GameAPIFunctions.GetContainerNumFreeSlotsAsync(i)
                                                                                     .ConfigureAwait(false)).bagType, objects.Find(obj => obj.Object == c)!.Name);
            }

            var bags = new List<Bag>(5);

            for (var i = 1; i <= containers.Count; i++)
                bags.Add(await createBag(containers[i], i)
                             .ConfigureAwait(false));

            bags.Insert(0, new Bag(joinAndRemoveFromList(player.BackpackItems), 16, (await m_game.GameAPIFunctions.GetContainerNumFreeSlotsAsync(0)
                                                                                                 .ConfigureAwait(false)).bagType, Name));

            return bags;
        }

        private async Task<IReadOnlyList<Item>> getInventoryAsync() {
            var player = (await m_player.ConfigureAwait(false)).Object.As<WoWAPI.Objects.Player>();

            return (await m_game.ObjectManager.GetObjectsAsync(true)
                                .ConfigureAwait(false)).Where(obj => obj.Common.Type == ObjectType.Item && player.BackpackItems.Contains(obj.Common.Guid))
                                                       .Select(obj => obj.Object.As<Item>())
                                                       .ToList();
        }

        #endregion Methods
    }
}