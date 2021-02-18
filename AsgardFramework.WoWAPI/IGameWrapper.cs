namespace AsgardFramework.WoWAPI
{
    public interface IGameWrapper
    {
        int ID { get; }
        IInjectedFunctions InjectedFunctions { get; }
        IGameAPIFunctions GameAPIFunctions { get; }
        IGameFunctions GameFunctions { get; }
        IObjectManager ObjectManager { get; }
        ISpellBook SpellBook { get; }
    }
}
