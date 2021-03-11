using System;

namespace AsgardFramework.Memory.Implementation
{
    public sealed class InterprocessManualResetEventSlim : SafeWaitHandleSlim
    {
        #region Constructors

        public InterprocessManualResetEventSlim(IntPtr handle, bool ownsHandle) : base(handle, ownsHandle) { }

        #endregion Constructors

        #region Methods

        public void ResetEvent() {
            if (!Kernel.ResetEvent(handle))
                throw new InvalidOperationException($"Can't reset event (error: 0x{Kernel.GetLastError():X}");
        }

        public void SetEvent() {
            if (!Kernel.SetEvent(handle))
                throw new InvalidOperationException($"Can't set event (error: 0x{Kernel.GetLastError():X}");
        }

        #endregion Methods
    }
}