using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace AsgardFramework.Memory.Implementation
{
    internal sealed class AutoScalingSharedBuffer : AutoManagedSharedBuffer, IAutoScalingSharedBuffer
    {
        #region Constructors

        internal AutoScalingSharedBuffer(IntPtr address, SafeHandle processHandle, int size, Func<int, AutoManagedSharedBuffer> fabric) : base(address, processHandle, size) {
            m_fabric = fabric;
        }

        #endregion Constructors

        #region Properties

        public override int Size => base.Size + m_additionalSize;

        #endregion Properties

        #region Fields

        private readonly List<AutoManagedSharedBuffer> m_additional = new List<AutoManagedSharedBuffer>();

        private readonly Func<int, AutoManagedSharedBuffer> m_fabric;

        private readonly object m_lock = new object();

        private int m_additionalSize;

        #endregion Fields

        #region Methods

        public IAutoManagedMemory Write<T>(T data) where T : new() {
            return WriteStruct(new object[] {
                data
            });
        }

        public IAutoManagedMemory Write<T>(T[] data) where T : new() {
            return WriteStruct(data.Cast<object>());
        }

        public IAutoManagedMemory WriteString(string value, Encoding encoding) {
            return WriteStruct(new object[] {
                value
            }, WideFieldsWriteType.ByVal, encoding);
        }

        public IAutoManagedMemory WriteStruct(IEnumerable<object> data, WideFieldsWriteType forWideFields = WideFieldsWriteType.ByVal, Encoding forStrings = default, int padding = 1) {
            forStrings ??= Encoding.Unicode;

            int calculateSizeRecursively(IEnumerable<object> __data) {
                var calculated = 0;

                foreach (var obj in __data)
                    switch (obj) {
                        case null:
                            calculated += 4;

                            break;

                        case IntPtr ptr:
                            calculated += 4;

                            break;

                        case Enum en:
                            calculated += 4;

                            break;

                        case string str:
                            if (forWideFields == WideFieldsWriteType.ByVal)
                                calculated += forStrings.GetByteCount(str + '\0');
                            else
                                calculated += 4;

                            break;

                        case IEnumerable<object> enumerable:
                            if (forWideFields == WideFieldsWriteType.ByVal)
                                calculated += calculateSizeRecursively(enumerable.ToArray());
                            else
                                calculated += 4;

                            break;

                        case IAutoManagedMemory pointer:
                            calculated += 4;

                            break;

                        default:
                            var sz = Marshal.SizeOf(obj.GetType());
                            sz = sz <= 4 ? 4 : sz;

                            if (forWideFields == WideFieldsWriteType.ByVal && sz > 4)
                                calculated += sz;
                            else
                                calculated += 4;

                            break;
                    }

                return calculated;
            }

            var size = forWideFields == WideFieldsWriteType.ByVal ? calculateSizeRecursively(data) : data.Count() * 4;
            var structure = new AutoManagedStructure(reserveBlock(size));

            int writeObjectsRecursively(IEnumerable<object> __data, AutoManagedStructure buffer, int offset) {
                foreach (var obj in __data)
                    switch (obj) {
                        case null:
                            buffer.Write(offset, 0);
                            offset += 4;

                            break;

                        case Enum en:
                            buffer.Write(offset, (int)Enum.ToObject(en.GetType(), en));
                            offset += 4;

                            break;

                        case string str:
                            var length = forStrings.GetByteCount(str + '\0');

                            if (forWideFields == WideFieldsWriteType.ByVal) {
                                buffer.WriteNullTerminatedString(offset, str, forStrings);
                                offset += length;
                            } else {
                                var ptr = Reserve(length);
                                ptr.WriteNullTerminatedString(0, str, forStrings);
                                buffer.Write(offset, ptr.Start);
                                buffer.AddNested(ptr);
                                offset += 4;
                            }

                            break;

                        case IEnumerable<object> enumerable:
                            var arr = enumerable.ToArray();

                            if (forWideFields == WideFieldsWriteType.ByVal) {
                                offset = writeObjectsRecursively(arr, buffer, offset);
                            } else {
                                var pStruct = new AutoManagedStructure(reserveBlock(calculateSizeRecursively(arr)));
                                writeObjectsRecursively(arr, pStruct, 0);
                                buffer.AddNested(pStruct);
                                buffer.Write(offset, pStruct.Start);
                                offset += 4;
                            }

                            break;

                        case IAutoManagedMemory pointer:
                            buffer.Write(offset, pointer.Start);
                            offset += 4;

                            break;

                        case IntPtr ptr:
                            buffer.Write(offset, (int)ptr);
                            offset += 4;

                            break;

                        default:
                            length = Marshal.SizeOf(obj.GetType());
                            var padSize = length < padding ? padding - length : 0;
                            var bytes = new byte[length + padSize];

                            unsafe {
                                fixed (byte* pbytes = bytes) {
                                    Marshal.StructureToPtr(obj, (IntPtr)pbytes + padSize, true);
                                }
                            }

                            if (forWideFields == WideFieldsWriteType.ByVal || bytes.Length <= 4) {
                                buffer.Write(offset, bytes);
                                offset += bytes.Length;
                            } else {
                                var ptr = Reserve(length);
                                ptr.Write(0, bytes);
                                buffer.Write(offset, ptr.Start);
                                buffer.AddNested(ptr);
                                offset += 4;
                            }

                            break;
                    }

                return offset;
            }

            writeObjectsRecursively(data, structure, 0);

            return structure;
        }

        protected internal override SharedBlock reserveBlock(int size) {
            lock (m_lock) {
                if (size <= base.Size && base.reserveBlock(size) is var reserved)
                    return reserved;

                foreach (var buffer in m_additional.Where(buffer => size <= buffer.Size)) {
                    reserved = buffer.reserveBlock(size);

                    if (reserved != null)
                        return reserved;
                }

                var newBuffer = m_fabric(size);
                m_additional.Insert(0, newBuffer);
                m_additionalSize += newBuffer.Size;

                return newBuffer.reserveBlock(size);
            }
        }

        #endregion Methods
    }
}