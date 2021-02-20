namespace AsgardFramework.WoWAPI
{
    public interface IGameWrapper
    {
        #region Properties

        IGameAPIFunctions GameAPIFunctions { get; }
        IGameFunctions GameFunctions { get; }
        int ID { get; }
        IInjectedFunctions InjectedFunctions { get; }
        IObjectManager ObjectManager { get; }
        ISpellBook SpellBook { get; }

        #endregion Properties
    }
}