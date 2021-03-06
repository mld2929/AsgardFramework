using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
            m_memory = new Lazy<IGlobalMemory>(() => new GlobalMemory(game.Id));
            var assembler = new Lazy<FasmAssembler>();
            var injector = new Lazy<DefaultCodeInjector>();
            var observer = new Lazy<DeviceObserver>(() => new DeviceObserver(m_memory.Value));

            var hook = new Lazy<EndSceneHookExecutor>(() => new EndSceneHookExecutor(injector.Value, m_memory.Value, observer.Value, assembler.Value));

            m_functions = new Lazy<FunctionsAccessor>(() => new FunctionsAccessor(hook.Value, assembler.Value, m_memory.Value));

            m_objectManager = new Lazy<IObjectManager>(() => new ObjectManager(m_memory.Value, m_functions.Value, m_functions.Value));
            m_spellBook = new Lazy<ISpellBook>(() => new SpellBook(m_memory.Value));
        }

        #endregion Constructors

        #region Fields

        private const int c_currentAccount = 0x00B6AA40;

        private const int c_currentRealm = 0x00C79B9E;

        private const int c_playerName = 0x00C79D18;

        private readonly Lazy<FunctionsAccessor> m_functions;

        private readonly Lazy<IGlobalMemory> m_memory;

        private readonly Lazy<IObjectManager> m_objectManager;

        private readonly Lazy<ISpellBook> m_spellBook;

        #endregion Fields

        #region Properties

        public static IReadOnlyList<IGameWrapper> Games =>
            Process.GetProcessesByName("Wow")
                   .Select(game => new GameWrapper(game))
                   .ToList();

        public string CurrentAccount => m_memory.Value.ReadNullTerminatedString(c_currentAccount, Encoding.UTF8);

        public string CurrentRealm => m_memory.Value.ReadNullTerminatedString(c_currentRealm, Encoding.UTF8);

        public IGameAPIFunctions GameAPIFunctions => m_functions.Value;

        public IGameFunctions GameFunctions => m_functions.Value;

        public int ID { get; }

        public IInjectedFunctions InjectedFunctions => m_functions.Value;

        public IObjectManager ObjectManager => m_objectManager.Value;

        public string PlayerName => m_memory.Value.ReadNullTerminatedString(c_playerName, Encoding.UTF8);

        public ISpellBook SpellBook => m_spellBook.Value;

        #endregion Properties

        #region Methods

        public static GameWrapper RunNew(Uri uri) {
            var game = Process.Start(uri.AbsolutePath);

            return new GameWrapper(game);
        }

        public void Dispose() {
            if (m_memory.IsValueCreated)
                m_memory.Value.Dispose();
        }

        #endregion Methods
    }
}