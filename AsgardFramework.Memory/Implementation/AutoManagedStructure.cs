using System.Collections.Generic;

namespace AsgardFramework.Memory.Implementation
{
    internal class AutoManagedStructure : SharedBlock
    {
        #region Fields

        private readonly List<IAutoManagedMemory> m_nested = new List<IAutoManagedMemory>();

        #endregion Fields

        #region Constructors

        internal AutoManagedStructure(SharedBlock wrapped) : base(wrapped.m_data, wrapped.m_processHandle) {
            m_nested.Add(wrapped);
        }

        #endregion Constructors

        #region Methods

        internal void AddNested(IAutoManagedMemory nested) {
            m_nested.Add(nested);
        }

        protected override bool ReleaseHandle() {
            for (var i = m_nested.Count - 1; i >= 0; i--)
                m_nested[i]
                    .Dispose();

            return true;
        }

        #endregion Methods
    }
}