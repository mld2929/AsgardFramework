using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal enum func_type
    {
        f_cdecl,
        f_std,
        f_this,
        f_virtual
    }

    internal sealed class EndSceneHookExecutor
    {
        #region Constructors

        internal EndSceneHookExecutor(IGlobalMemory memory) {
            m_memory = memory;
            m_buffer = m_memory.AllocateAutoScalingShared(4096);
            m_queue = m_memory.Allocate(4096);

            var data = c_functions.Select(pair => new object[] {
                                      pair.Key,
                                      pair.Value.address,
                                      pair.Value.type,
                                      pair.Value.argsCount
                                  })
                                  .ToArray();

            m_core = m_memory.LoadDll(AppDomain.CurrentDomain.BaseDirectory + c_coreName, c_coreName);

            Console.ReadKey();
            m_event = new InterprocessManualResetEventSlim((IntPtr)m_core[c_init, false, Encoding.UTF8](data, data.Length, m_queue), false);
        }

        #endregion Constructors

        #region Fields

        private const string c_coreName = "AsgardFramework.Core.dll";
        private const string c_init = "InitInteraction";

        private static readonly Dictionary<string, (int address, func_type type, int argsCount)> c_functions = new Dictionary<string, (int address, func_type type, int argsCount)> {
            {
                "RunScript", (0x004DD490, func_type.f_cdecl, 2)
            }, {
                "PushLString", (0x0084E350, func_type.f_cdecl, 2)
            }
        };

        private readonly IAutoScalingSharedBuffer m_buffer;
        private readonly InterprocessManualResetEventSlim m_event;

        private readonly IGlobalMemory m_memory;
        private readonly IAutoManagedMemory m_queue;
        private readonly int m_queueCapacity;
        private readonly IDll m_core;

        #endregion Fields
    }
}