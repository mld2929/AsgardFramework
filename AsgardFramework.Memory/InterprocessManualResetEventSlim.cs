using System;

namespace AsgardFramework.Memory
{
    public sealed class InterprocessManualResetEventSlim : SafeWaitHandleSlim
    {
        #region Constructors

        public InterprocessManualResetEventSlim(IntPtr handle, bool ownsHandle) : base(handle, ownsHandle) { }

        #endregion Constructors

        #region Methods

        public void ResetEvent() {
            Kernel.ResetEvent(handle);
        }

        public void SetEvent() {
            Kernel.SetEvent(handle);
        }

        #endregion Methods
    }
}