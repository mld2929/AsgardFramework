using System;

namespace AsgardFramework.WoWAPI
{
    public interface IGameWrapper : IDisposable
    {
        #region Properties

        string CurrentAccount { get; }
        string CurrentRealm { get; }
        IGameAPIFunctions GameAPIFunctions { get; }
        IGameFunctions GameFunctions { get; }
        int ID { get; }
        IInjectedFunctions InjectedFunctions { get; }
        ILuaScriptExecutor LuaExecutor { get; }
        IObjectManager ObjectManager { get; }
        string PlayerName { get; }
        ISpellBook SpellBook { get; }

        #endregion Properties
    }
}