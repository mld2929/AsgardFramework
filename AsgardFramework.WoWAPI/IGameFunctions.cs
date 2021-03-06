using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    public interface IGameFunctions
    {
        #region Methods

        Task AscendStopAsync();

        Task CastSpellAsync(int spellId, ulong target);

        Task ClickToMoveAsync(float x,
                              float y,
                              float z,
                              int ctmState,
                              ulong playerOrTargetGuid,
                              float precision);

        Task ClickToMoveStopAsync();

        Task<int> GetItemIdByNameAsync(string itemName);

        Task<string> GetNameAsync(int objBase);

        Task<string> GetPlayerNameAsync();

        Task<Position> GetPositionAsync(int objBase);

        Task<bool> HasAuraBySpellIdAsync(int objBase, int spellId);

        Task InteractAsync(int objBase);

        Task JumpOrAscendStartAsync();

        Task MoveBackwardStartAsync();

        Task MoveBackwardStopAsync();

        Task MoveForwardStartAsync();

        Task MoveForwardStopAsync();

        Task ObjectOnClickAsync(int objBase);

        Task SellItemAsync(ulong itemGuid, ulong vendorGuid);

        Task StartEnterWorldAsync();

        Task StartLoginToDefaultServerAsync(string login, string password);

        Task TargetUnitAsync(ulong guid);

        Task TurnLeftStartAsync();

        Task TurnLeftStopAsync();

        Task TurnRightStartAsync();

        Task TurnRightStopAsync();

        Task UnitOnClickAsync(int unitBase);

        Task UseItemAsync(int itemBase, ulong itemGuid);

        #endregion Methods
    }
}