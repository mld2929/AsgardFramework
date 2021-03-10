using System;
using System.Threading.Tasks;

using Microsoft.Win32.SafeHandles;

namespace AsgardFramework.Memory
{
    public class SafeWaitHandleSlim : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Constructors

        public SafeWaitHandleSlim() : base(true) { }

        public SafeWaitHandleSlim(IntPtr handle, bool ownsHandle) : base(ownsHandle) {
            SetHandle(handle);

            if (IsInvalid)
                throw new ArgumentException("Invalid handle");
        }

        #endregion Constructors

        #region Methods

        public async Task WaitForSignalAsync() {
            while (!WaitForSingleObject(0))
                await Task.Yield();
        }

        /// <summary>
        ///     The WaitForSingleObject function checks the current state of the object. If the object's state is nonsignaled, the
        ///     calling thread enters the wait state until the object is signaled or the time-out interval elapses.
        /// </summary>
        /// <param name="milliseconds">
        ///     The time-out interval, in milliseconds. If a nonzero value is specified, the function waits
        ///     until the object is signaled or the interval elapses. If is zero, the function does not enter a wait state if the
        ///     object is not signaled; it always returns immediately. If is -1, the function will return only
        ///     when the object is signaled.
        /// </param>
        /// <returns><see langword="true" /> if signaled; otherwise <see langword="false" /></returns>
        public bool WaitForSingleObject(int milliseconds) {
            var code = Kernel.WaitForSingleObject(handle, milliseconds);

            if (code == -1)
                throw new InvalidOperationException($"Can't wait on given handle (error: 0x{Kernel.GetLastError():X})");

            return code == 0;
        }

        protected override bool ReleaseHandle() {
            return Kernel.CloseHandle(handle);
        }

        #endregion Methods
    }
}