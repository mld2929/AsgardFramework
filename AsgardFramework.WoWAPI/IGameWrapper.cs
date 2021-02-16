namespace AsgardFramework.WoWAPI
{
    public interface IGameWrapper
    {
        IFunctions Functions { get; }
        IObjectManager ObjectManager { get; }
        ISpellBook SpellBook { get; }
    }
}
