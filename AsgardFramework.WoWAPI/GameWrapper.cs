using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AsgardFramework.WoWAPI
{
    public class GameWrapper : IGameWrapper
    {
        public static IReadOnlyList<IGameWrapper> Games =>
                Process.GetProcessesByName("Wow").Select(game => new GameWrapper(game)).ToList();

        private readonly Lazy<IFunctions> m_functions;
        private readonly Lazy<IObjectManager> m_objectManager;
        private readonly Lazy<ISpellBook> m_spellBook;
        public IFunctions Functions => m_functions.Value;

        public IObjectManager ObjectManager => m_objectManager.Value;

        public ISpellBook SpellBook => m_spellBook.Value;

        public int ID { get; private set; }

        public static GameWrapper RunNew(Uri uri)
        {
            var game = Process.Start(uri.AbsoluteUri);
            return new GameWrapper(game);
        }





        private GameWrapper(Process game)
        {
            ID = game.Id;
            var memory = new Lazy<GlobalMemory>(() => new GlobalMemory(game.Id));
            var compiler = new Lazy<FasmCompiler>();
            var injector = new Lazy<CodeInjector>();
            var observer = new Lazy<DeviceObserver>(() => new DeviceObserver(memory.Value));
            var hook = new Lazy<EndSceneHookExecutor>(() => new EndSceneHookExecutor(injector.Value, memory.Value, observer.Value, compiler.Value));
            m_functions = new Lazy<IFunctions>(() => new FunctionsAccessor(hook.Value, compiler.Value));
            m_objectManager = new Lazy<IObjectManager>(() => new ObjectManager(memory.Value, m_functions.Value));
        }

        
    }
}
