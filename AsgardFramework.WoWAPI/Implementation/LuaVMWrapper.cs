using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsgardFramework.Memory;
using AsgardFramework.WoWAPI.LuaData;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class LuaVMWrapper : ILuaScriptExecutor
    {
        #region Constructors

        internal LuaVMWrapper(IMainThreadExecutor executor, IGlobalMemory memory) {
            m_executor = executor;
            m_memory = memory;
            m_executor.RegisterFunctions(c_functions);
            m_luaTop = m_executor[c_getTop, -1]();
        }

        #endregion Constructors

        #region Fields

        private const string c_getText = "lua_getText";

        private const string c_getTop = "lua_getTop";

        private const string c_pushString = "lua_pushLString";

        private const string c_runScript = "lua_runScript";

        private const string c_setTop = "lua_setTop";

        private static readonly IReadOnlyList<(string functionName, FunctionCallType functionType, int functionAddress, int argumentsCount)> c_functions = new[] {
            (c_setTop, FunctionCallType.Cdecl, 0x0084DBF0, 2),
            (c_getTop, FunctionCallType.Cdecl, 0x00817DB0, 0),
            (c_runScript, FunctionCallType.Cdecl, 0x004DD490, 1),
            (c_getText, FunctionCallType.Cdecl, 0x00819D40, 3),
            (c_pushString, FunctionCallType.Cdecl, 0x0084E350, 2)
        };

        private readonly IMainThreadExecutor m_executor;

        private readonly int m_luaTop;

        private readonly IGlobalMemory m_memory;

        #endregion Fields

        #region Methods

        public Task RunScriptAsync(string script) {
            var executionList = new[] {
                c_pushString,
                c_runScript,
                c_setTop
            };

            return m_executor[executionList](new object[] {
                script,
                m_luaTop
            }, new object[] {
                m_luaTop
            }, new object[] {
                0,
                m_luaTop
            });
        }

        public async Task<T> RunScriptAsync<T>(string script, int returnCount = 10) where T : LuaValue, new() {
            var vars = new List<string>();
            var sb = new StringBuilder(returnCount * 3);

            for (var i = 0; i < returnCount; i++)
                vars.Add($"v{i}");

            sb.AppendJoin(", ", vars);
            sb.Append(" = ");
            sb.Append(script);

            return (T)new T().Parse((await RunScriptAsync(sb.ToString(), vars)
                                         .ConfigureAwait(false)).ToArray());
        }

        public async Task<IReadOnlyList<string>> RunScriptAsync(string script, IEnumerable<string> returnVariables) {
            var executionList = new List<string> {
                c_pushString,
                c_runScript
            };

            executionList.AddRange(returnVariables.Select(v => c_getText));
            executionList.Add(c_setTop);

            var executionArgs = new List<object[]> {
                new object[] {
                    script,
                    m_luaTop
                },
                new object[] {
                    m_luaTop
                }
            };

            executionArgs.AddRange(returnVariables.Select(v => new object[] {
                0,
                -1,
                v
            }));

            executionArgs.Add(new object[] {
                0,
                m_luaTop
            });

            return (await m_executor[executionList](executionArgs.ToArray())
                        .ConfigureAwait(false)).Take(executionList.Count - 1)
                                               .TakeLast(returnVariables.Count())
                                               .Select(ptr => m_memory.ReadNullTerminatedString(ptr, Encoding.UTF8))
                                               .ToList();
        }

        #endregion Methods
    }
}