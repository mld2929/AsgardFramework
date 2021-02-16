namespace AsgardFramework.WoWAPI
{
    public interface IGameWrapper
    {
        int ID { get; }
        IFunctions Functions { get; }
        IObjectManager ObjectManager { get; }
        ISpellBook SpellBook { get; }
    }
}
