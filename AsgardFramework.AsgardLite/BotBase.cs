using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI;
using AsgardFramework.WoWAPI.LuaData;

namespace AsgardFramework.AsgardLite
{
    public abstract class BotBase
    {
        #region Constructors

        protected BotBase(IGameWrapper game, BagType? required = null, Dictionary<BotConditions, Func<Task<bool>>> settings = null) {
            m_game = game;
            m_player = new Player(game);
            m_requiredBagType = required;
            m_settings = settings ?? DefaultSettings;
        }

        #endregion Constructors

        #region Fields

        protected static readonly Dictionary<BotConditions, Func<Task<bool>>> DefaultSettings = Enum.GetValues(typeof(BotConditions))
                                                                                                    .Cast<BotConditions>()
                                                                                                    .ToDictionary(condition => condition,
                                                                                                                  condition => condition == BotConditions.Normal ?
                                                                                                                                   new Func<Task<bool>>(async () => true) : async () => false);

        protected readonly IGameWrapper m_game;

        protected readonly Player m_player;

        protected readonly BagType? m_requiredBagType;

        protected readonly Dictionary<BotConditions, Func<Task<bool>>> m_settings;

        protected volatile Task m_conditions = Task.CompletedTask;

        #endregion Fields

        #region Methods

        public async Task Run(CancellationToken token) {
            await iniIfNeededAsync()
                .ConfigureAwait(false);

            conditionsWatcher(token);

            while (!token.IsCancellationRequested)
                await normalActions(token)
                    .ConfigureAwait(false);
        }

        protected virtual async void conditionsWatcher(CancellationToken token) {
            while (!token.IsCancellationRequested) {
                var task = m_settings[await getCurrentCondition(token)
                                          .ConfigureAwait(false)]();

                m_conditions = task;

                if (!await task.ConfigureAwait(false))
                    break;
            }

            await m_settings[BotConditions.Stop]()
                .ConfigureAwait(false);
        }

        protected virtual async Task<BotConditions> getCurrentCondition(CancellationToken token) {
            if (await m_player.InCombat.ConfigureAwait(false))
                return BotConditions.Combat;

            if (!await m_player.IsConnected.ConfigureAwait(false))
                return BotConditions.Disconnect;

            if (await m_player.IsDead.ConfigureAwait(false))
                return BotConditions.Dead;

            if (m_requiredBagType != null &&
                await m_player.GetFreeSlotsFor(m_requiredBagType.Value)
                              .ConfigureAwait(false) ==
                0)
                return BotConditions.BagsFull;

            return BotConditions.Normal;
        }

        protected virtual async Task iniIfNeededAsync() { }

        protected abstract Task normalActions(CancellationToken token);

        #endregion Methods
    }
}