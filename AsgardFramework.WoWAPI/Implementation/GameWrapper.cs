using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using AsgardFramework.CodeInject;
using AsgardFramework.FasmManaged;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    public sealed class GameWrapper : IGameWrapper
    {
        #region Constructors

        private GameWrapper(Process game) {
            ID = game.Id;
            var memory = new Lazy<GlobalMemory>(() => new GlobalMemory(game.Id));
            var assembler = new Lazy<FasmAssembler>();
            var injector = new Lazy<DefaultCodeInjector>();
            var observer = new Lazy<DeviceObserver>(() => new DeviceObserver(memory.Value));

            var hook = new Lazy<EndSceneHookExecutor>(() => new EndSceneHookExecutor(injector.Value, memory.Value, observer.Value, assembler.Value));

            m_functions = new Lazy<FunctionsAccessor>(() => new FunctionsAccessor(hook.Value, assembler.Value, memory.Value));

            m_objectManager = new Lazy<IObjectManager>(() => new ObjectManager(memory.Value, m_functions.Value));
            m_spellBook = new Lazy<ISpellBook>(() => new SpellBook(memory.Value));
        }

        #endregion Constructors

        #region Methods

        public static GameWrapper RunNew(Uri uri) {
            var game = Process.Start(uri.AbsolutePath);

            return new GameWrapper(game);
        }

        #endregion Methods

        #region Fields

        private readonly Lazy<FunctionsAccessor> m_functions;

        private readonly Lazy<IObjectManager> m_objectManager;

        private readonly Lazy<ISpellBook> m_spellBook;

        #endregion Fields

        #region Properties

        public static IReadOnlyList<IGameWrapper> Games =>
            Process.GetProcessesByName("Wow")
                   .Select(game => new GameWrapper(game))
                   .ToList();

        public IGameAPIFunctions GameAPIFunctions => m_functions.Value;

        public IGameFunctions GameFunctions => m_functions.Value;

        public int ID { get; }

        public IInjectedFunctions InjectedFunctions => m_functions.Value;

        public IObjectManager ObjectManager => m_objectManager.Value;

        public ISpellBook SpellBook => m_spellBook.Value;

        #endregion Properties
    }
}