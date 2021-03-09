using System.Threading;
using System.Threading.Tasks;

using AsgardFramework.WoWAPI;
using AsgardFramework.WoWAPI.LuaData;
using AsgardFramework.WoWAPI.Objects;
using AsgardFramework.WoWAPI.Utils;

namespace AsgardFramework.AsgardLite
{
    internal class FishBot : BotBase
    {
        #region Fields

        private string fishingSpell;

        #endregion Fields

        #region Constructors

        public FishBot(IGameWrapper game) : base(game, BagType.Default) { }

        #endregion Constructors

        #region Methods

        protected override async Task iniIfNeededAsync() {
            fishingSpell ??= (await m_game.GameAPIFunctions.GetSpellInfoAsync(62734)
                                          .ConfigureAwait(false)).Name;
        }

        protected override async Task normalActions(CancellationToken token) {
            await m_player.CastSpellAsync(fishingSpell)
                          .ConfigureAwait(false);

            if (token.IsCancellationRequested)
                return;

            var bobberGuid = (await m_game.ObjectManager.GetPlayerAsync()
                                          .ConfigureAwait(false)).Object.As<WoWAPI.Objects.Player>()
                                                                 .ChannelObject;

            if (token.IsCancellationRequested || bobberGuid == 0)
                return;

            async Task<(GameObject bobber, int objBase)> getBobber() {
                if (token.IsCancellationRequested)
                    return (null, 0);

                var bb = await m_game.ObjectManager.GetObjectByGuidAsync(bobberGuid)
                                     .ConfigureAwait(false);

                return token.IsCancellationRequested ? (null, 0) : (bb.Object.As<GameObject>(), bb.Base);
            }

            var bobber = await getBobber()
                             .ConfigureAwait(false);

            bool isBobbing() {
                return (bobber.bobber.Flags & 0x20) == 1;
            }

            while (bobber.bobber != null && !isBobbing()) {
                await Task.Delay(500)
                          .ConfigureAwait(false);

                if (token.IsCancellationRequested)
                    return;

                bobber = await getBobber()
                             .ConfigureAwait(false);
            }

            if (bobber.bobber != null)
                await m_game.GameFunctions.InteractAsync(bobber.objBase)
                            .ContinueWith(_ => Task.Delay(500))
                            .ConfigureAwait(false);

            var slots = await m_game.GameAPIFunctions.GetNumLootItemsAsync()
                                    .ConfigureAwait(false);
        }

        #endregion Methods
    }
}