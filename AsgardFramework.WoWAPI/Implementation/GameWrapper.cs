using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    public class GameWrapper : IGameWrapper
    {
        public static IReadOnlyList<IGameWrapper> Games =>
                Process.GetProcessesByName("Wow").Select(game => new GameWrapper(game)).ToList();

        private readonly Lazy<FunctionsAccessor> m_functions;
        private readonly Lazy<IObjectManager> m_objectManager;
        private readonly Lazy<ISpellBook> m_spellBook;
        public IInjectedFunctions InjectedFunctions => m_functions.Value;

        public IObjectManager ObjectManager => m_objectManager.Value;

        public ISpellBook SpellBook => m_spellBook.Value;

        public int ID { get; private set; }

        public IGameAPIFunctions GameAPIFunctions => m_functions.Value;

        public IGameFunctions GameFunctions => m_functions.Value;

        public static GameWrapper RunNew(Uri uri) {
            var game = Process.Start(uri.AbsolutePath);
            return new GameWrapper(game);
        }

        private GameWrapper(Process game) {
            ID = game.Id;
            var memory = new Lazy<GlobalMemory>(() => new GlobalMemory(game.Id));
            var compiler = new Lazy<FasmAssembler>();
            var injector = new Lazy<DefaultCodeInjector>();
            var observer = new Lazy<DeviceObserver>(() => new DeviceObserver(memory.Value));
            var hook = new Lazy<EndSceneHookExecutor>(() => new EndSceneHookExecutor(injector.Value, memory.Value, observer.Value, compiler.Value));
            m_functions = new Lazy<FunctionsAccessor>(() => new FunctionsAccessor(hook.Value, compiler.Value, memory.Value));
            m_objectManager = new Lazy<IObjectManager>(() => new ObjectManager(memory.Value, m_functions.Value));
            m_spellBook = new Lazy<ISpellBook>(() => new SpellBook(memory.Value));
        }


    }
}
