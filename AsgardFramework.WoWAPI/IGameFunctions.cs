using System.Threading.Tasks;

using AsgardFramework.WoWAPI.Objects;

namespace AsgardFramework.WoWAPI
{
    public interface IGameFunctions
    {
        Task ClickToMove(float x, float y, float z, int ctmState, ulong playerOrTargetGuid, float precision);
        Task JumpOrAscendStart();
        Task AscendStop();
        Task MoveForwardStart();
        Task MoveForwardStop();
        Task MoveBackwardStart();
        Task MoveBackwardStop();
        Task TurnRightStart();
        Task TurnRightStop();
        Task TurnLeftStart();
        Task TurnLeftStop();
        Task UnitOnClick(int unitBase);
        Task ObjectOnClick(int objBase);
        Task Target(ulong guid);
        Task CastSpell(int spellId, ulong target);
        Task Interact(int objBase);
        Task<string> GetName(int objBase);
        Task<Position> GetPosition(int objBase);
        Task SellItem(ulong itemGuid, ulong vendorGuid);
        Task StartLoginToDefaultServer(string login, string password);
        Task UseItem(int itemBase, ulong itemGuid);
        Task<int> GetItemIdByName(string itemName);
        Task<string> GetPlayerName();

        Task<bool> HasAuraBySpellId(int objBase, int spellId);

        Task StartEnterWorld();
    }
}
