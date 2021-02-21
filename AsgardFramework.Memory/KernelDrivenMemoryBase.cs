using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace AsgardFramework.Memory
{
    public abstract class KernelDrivenMemoryBase : SafeHandle, IMemory
    {
        #region Fields

        protected SafeHandle m_processHandle;

        #endregion Fields

        #region Constructors

        protected KernelDrivenMemoryBase(SafeHandle processHandle) : base(IntPtr.Zero, true) {
            m_processHandle = processHandle;

            if (m_processHandle.IsInvalid)
                throw new ArgumentException("Handle is invalid", nameof(processHandle));

            var weakThis = new WeakReference<IMemory>(this);

            AppDomain.CurrentDomain.ProcessExit += (_, __) => {
                if (weakThis.TryGetTarget(out var @this) && !@this.Disposed)

                    @this.Dispose();
            };
        }

        #endregion Constructors

        #region Events

        public event EventHandler Disposing = (sender, args) => { };

        #endregion Events

        #region Properties

        public virtual bool Disposed => IsClosed;

        #endregion Properties

        #region Methods

        public virtual byte[] Read(int offset, int count) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(KernelDrivenMemoryBase));

            var buffer = new byte[count];
            Kernel.ReadProcessMemory(m_processHandle, offset, buffer, count, out _);

            return buffer;
        }

        public T Read<T>(int offset) where T : new() {
            if (Disposed)
                throw new ObjectDisposedException(nameof(KernelDrivenMemoryBase));

            if (typeof(T) == typeof(string)) {
                var str = ReadString(offset, Encoding.UTF8);

                return Unsafe.As<string, T>(ref str);
            }

            unsafe {
                fixed (byte* buffer = Read(offset, Marshal.SizeOf<T>())) {
                    return Marshal.PtrToStructure<T>((IntPtr)buffer);
                }
            }
        }

        public string ReadString(int offset, Encoding encoding) {
            var maxCharSize = encoding.GetMaxByteCount(1);
            var bytes = Read(offset, maxCharSize);

            if (bytes.All(b => b == 0))
                return string.Empty;

            var buffer = new List<byte>(maxCharSize * 256);

            do {
                buffer.AddRange(bytes);
                offset += maxCharSize;
                bytes = Read(offset, maxCharSize);
            } while (bytes.Any(b => b != 0));

            return encoding.GetString(buffer.ToArray())
                           .Trim();
        }

        public virtual void Write(int offset, byte[] data) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(KernelDrivenMemoryBase));

            Kernel.WriteProcessMemory(m_processHandle, offset, data, data.Length, out _);
        }

        public void Write<T>(int offset, T data) where T : new() {
            if (typeof(T) == typeof(string))
                WriteString(offset, data as string, Encoding.UTF8);

            var bytes = new byte[Marshal.SizeOf<T>()];

            unsafe {
                fixed (byte* buffer = bytes) {
                    Marshal.StructureToPtr(data, (IntPtr)buffer, true);
                }
            }

            Write(offset, bytes);
        }

        public void WriteString(int offset, string data, Encoding encoding) {
            Write(offset, encoding.GetBytes(data));
        }

        protected override void Dispose(bool disposing) {
            Disposing(this, EventArgs.Empty);
            base.Dispose(disposing);
        }

        #endregion Methods
    }
}