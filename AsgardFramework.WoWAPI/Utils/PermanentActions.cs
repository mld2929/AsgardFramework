using System.Collections.Generic;

using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Utils
{ // todo: rewrite
    internal class PermanentActions
    {
        #region Constructors

        internal PermanentActions(IAutoScalingSharedBuffer buffer) {
            m_buffer = buffer;
            m_callListSize = m_buffer.Size;
            //addNewCallListIfNeeded();
        }

        #endregion Constructors

        #region Properties

        internal int ActionsStartAddress =>
            m_callLists[0]
                .Start;

        #endregion Properties

        #region Fields

        private static readonly byte c_nop = 0x90;

        private static readonly byte c_ret = 0xC3;

        private readonly IAutoScalingSharedBuffer m_buffer;

        private readonly List<IAutoManagedMemory> m_callLists = new List<IAutoManagedMemory>();

        private readonly int m_callListSize;

        private int m_actionsLeft;

        private int m_currentOffset;

        #endregion Fields

        //internal void Add(ICodeBlock code) {
        //    addNewCallListIfNeeded();
        //    var list = m_callLists.Last();

        //    if (!m_buffer.TryReserve(code.Compiled.Length, out var buffer))
        //        throw new InvalidOperationException("Can't reserve memory");

        //    m_injector.Inject(buffer, code, 0);
        //    var call = getCall(buffer.Start);
        //    m_injector.InjectWithoutRet(list, call, m_currentOffset);
        //    m_actionsLeft--;
        //    m_currentOffset += call.Compiled.Length;
        //}

        //private void addNewCallListIfNeeded() {
        //    if (m_actionsLeft != 0)
        //        return;

        //    var reserved = m_buffer.Reserve(m_callListSize);

        //    m_actionsLeft = reserved.Size / 7 - 2; // 1 byte for ret, 7 bytes for new list call
        //    var bytes = new byte[reserved.Size];
        //    Array.Fill(bytes, c_nop);
        //    reserved[..] = bytes;

        //    reserved[^1] = c_ret;

        //    if (m_callLists.LastOrDefault() is IAutoManagedMemory list) {
        //        var call = getCall(reserved.Start);
        //        m_injector.InjectWithoutRet(list, call, m_currentOffset);
        //    }

        //    m_currentOffset = 0;
        //    m_callLists.Add(reserved);
        //}

        //private ICodeBlock getCall(int to) {
        //    return m_assembler.Assemble(to.CallViaEax())
        //                      .ToCodeBlock();
        //}
    }
}