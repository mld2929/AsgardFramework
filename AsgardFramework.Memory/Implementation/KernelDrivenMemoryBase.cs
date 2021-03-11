using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace AsgardFramework.Memory.Implementation
{
    public abstract class KernelDrivenMemoryBase : SafeHandle, IMemory
    {
        #region Fields

        protected internal SafeHandle m_processHandle;

        #endregion Fields

        #region Constructors

        protected KernelDrivenMemoryBase(SafeHandle processHandle, IntPtr handle) : base(IntPtr.Zero, true) {
            m_processHandle = processHandle;
            SetHandle(handle);

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

        public SafeHandle GetHandle() {
            return this;
        }

        public byte Read(int offset) {
            return Read(offset, 1)[0];
        }

        public virtual byte[] Read(int offset, int count) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(KernelDrivenMemoryBase));

            var buffer = new byte[count];

            if (!Kernel.ReadProcessMemory(m_processHandle, offset, buffer, count, out var actual) || actual != count)
                throw new InvalidOperationException($"Can't read bytes at 0x{offset:X} (read {actual}/{count}; error: 0x{Kernel.GetLastError():X})");

            return buffer;
        }

        public T Read<T>(int offset) where T : new() {
            if (Disposed)
                throw new ObjectDisposedException(nameof(KernelDrivenMemoryBase));

            unsafe {
                fixed (byte* buffer = Read(offset, Marshal.SizeOf<T>())) {
                    return Marshal.PtrToStructure<T>((IntPtr)buffer);
                }
            }
        }

        public T[] Read<T>(int offset, int count) where T : new() {
            var size = Marshal.SizeOf<T>();

            unsafe {
                fixed (byte* buffer = Read(offset, size * count)) {
                    var result = new T[count];

                    for (var i = 0; i < count; i++)
                        result[i] = Unsafe.AsRef<T>(buffer + i * size);

                    return result;
                }
            }
        }

        public string ReadNullTerminatedString(int offset, Encoding encoding) {
            var maxCharSize = encoding.GetMaxByteCount(1);
            var bytes = Read(offset, maxCharSize);

            var terminator = encoding.GetBytes(new[] {
                '\0'
            });

            if (bytes.SequenceEqual(terminator))
                return string.Empty;

            var buffer = new List<byte>(bytes);

            while (!bytes.Intersect(terminator)
                         .Any()) {
                offset += maxCharSize;
                bytes = Read(offset, maxCharSize);
                buffer.AddRange(bytes);
            }

            var str = encoding.GetString(buffer.ToArray());
            var index = str.IndexOf('\0');

            if (index != -1)
                // -1 is impossible?

                //                                                                 `...........  `
                //                                                            ,;+zxxxnnnnnnnnnMxWWn+i,`
                //                                                        .+xMn#*i:::;::::::::+*:*z@Mnxn*.
                //                                                    `:*xM*:::::::::nn*::::::;#M+::*xz**nz,
                //                                                 .izMz+;:::xn+i:::::;zMx+;:::::#x;::inx;inz`
                //                                              :+nz+ii:::::::i++zzz*::::*+#n+;:::;n#:::izni;zi
                //                                           .;zzz*:::#n#+;::::::::i#n#*:::::+nz#;::+n*:::;n#;z*
                //                                         .#x#::i#xn+::i+xx+:::::::::;zM+::::::*x+:::zx+:::iM:zi
                //                                       `#x*znn;:::;#xi::::#Mzi:::::::::#M#::::::#n::::zx+::+n:x;
                //                                      ;M*::;;inn::::;xz:::::*nn*:::::::::+x+:::::iMi::::+xn;n*;x`
                //                                     ix;:::::::ix*::::*x;:::::izxzi::::::::zxi::::;x#:::::in#W:+*
                //                                    `Min*::::::::n#::::;x+:::::::*zx#;::::::;nni::::n+::::::x+x:x`
                //                                    **:;#xni::::::#n;::::zn;::::::::*nn;::::::inz::::n*:::::*nz#+*
                //                                   .n:::::ixz::::::*x;::::*xn;::::::::in+:::::::*x#:::nz;::::xiM;n
                //                                   #M*;;::::*z;:::::ixz;::::ix+:::::::::#x*:::::::*M+::inx+::ixiMz,
                //                                   zi#nxn*:::*x+::::::+xi:::::+x#;:::::::;#xx#i:::::+x+:::zz::+nzM;
                //                                   zn*#,:*n;:::+n;:::::;#n;:::::*zx+:::::::::i+n;i#i::+nn#iz*i:#zM+
                //                                   #W##n:::x+:::;xz;:::::iM+;:*+i;;+nn::+zzzzzzzzWzzxnMz:*zxn@xx#Mx
                //                                  .M.#M*x+::zz;:::*x#::Wn*i#xzx#znxxnnzx@iiiiiiiii:,,:i*,,,,:i,:i:x+
                //                                  z+..;M#+n;,zWxn#i:+x+#M@znMMM+,,,,;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,;@,
                //                                 ,W,....inMWi:++,,*+#nMx#+,,,,::,,,,,,,,::::,,,,,,,,:::::,,,,,,:::,#n
                //                                 +#.......;n@*:zi......,M,,,::::,,,,:,:::::::,:,,,:,:::::::,,:::::::W.
                //                                 n,.........:Mn:M;.....iz,::::::,,,,:::::::::,,,,,,,:::::::,,,,::::,n*
                //                                ,n...........,zn;x.....*#:::::,,:,,,,::::::::,:,,,,,,,,:,,,,,,,:::::;M
                //                                #*.............izz,....iz:::::,,,,,,:::::::::,:,,,,,,,,,,,,,,,,,,:::,ni
                //                               .@:...,,i;,,.....+*;....:x,:::::,,,::::::::::::,::,,,,,,,,,,:,,,,:::,,iM`
                //                               zn,inxMMznMMxn#*;,z:....,M,,::::,,:::::::::::::::::::::,,,,::,,,,,:::,,W:
                //                              *W,++:.........:*+i.......W;,,::,,:::::::,,:::,,:::::::::,,,:::,,,,,,::,*x
                //                              M*...................*....##,,::,,:::::,,,,,,,,,,::::::::,,:::::,,,,,:,:,M,
                //                             ,M....................x;...,@,,,:,,::::,,,,,,,,,,,:::::::::,::,,:,,,,,,,:,x:
                //                             *+....................#+....z+,,,,:,:,;znz;,,,,,,,,:::,::,,,,:,::::,:,,,,,x:
                //                             z+.z:...,+nn#*,.......i#....ix,,,:,,:zx;izW;,:,,,,::::::::,,,::::::::,,,,,x:
                //                            .@#.Wz*+nM#;:i+xn:.....,n.....M,,,,*x@#....+x,,,,,,::::::::,,,::::::::,,,,,x*
                //                            ,nW;x++*i#znzzz#*zz,...,n.....x;,,#MM+......xi,:,:,::,::::::,,::::::::,,,,,xz
                //                             xnni#;zW+,....;zxxW;..:#.....n+,*x,x.......in,,::,::::::::,,,::::::::,,,,,xz
                //                             .zWizxinWzzzzz+:.*nW#,.......+x,+#.i.xxx#,.:#:::::,::::::::,,,,:::::,,,,,,xz
                //                              `W:....,Wnzz@#x#...ixi......;z,:xi..,..ix:.Wi:::::::::::::,,::,::,,,,,,,,xz
                //                              in..;,.,@Mx*x.`+n:#+:,......:M*z:Mi.....,n,#+,,:,,::::::,,,,,,:,,,,,,,,,,xz
                //                              M:..W,.:WW@*z:.+x,i#nn......:@,++iW:,*+;.*+*z,,,,::::,,,,,,:,,,:,:,,,,,,,x#
                //                             ;n..in...M#**nzMz,............@i,#zzMM##zM:n:z,,,:::::,,,,,,:,,,,,,,,,,,,,M*
                //                             M;..zi..,W@W@Wz:..............W#;:iz#z...:.n:z,,,,,:::,,:,,,:,,:,,,,,,,,,:@,
                //                            :x...x.........................M#+*,:;W....,n:z,,,,::::::,::,,,::,,,,,,,,,*x
                //                            ni..;z.........................W;:n:,:W....i#:z,,,::,,,::,:,::,::,,,,,,,,,W:
                //                           .W...zi.......................,xW;:in:,W....#*:z,,:::::,,:::,,,,:,,,:,,,,,#z
                //                           i+...W........................ziinziix*M,...M,*#,,:::::,,:,,:::::,,,::,,,;W.
                //                           n,...:.....................*+#z:,zn#,:+W:..:*.x:,:::::::::,::::::::,::,,,Mi
                //                          ,x..........................i@nx:n:Mi:,:M,....*z,,::::::,,::::::::,:,,:,,*x`
                //                          z*.........................#M+:##:::+xx#M:...:M,,:::::,,,,::::::::,,,,,,#M.
                //                         `W,........:,..............,nni,:#z::::::zi..:Mi,::::,,,,,,,,:::,,,,,,,,z#`
                //                         ;n........:znx:............+*#*:+x*xi,*n:iz.,Wx,:::::,,,,,,,,,,,,,,,,,,x#
                //                         i#...........;M...........iW:;x::;::z::ix;@*x*z,:::,,,,,,,,,,,,,,::::,x#
                //                         ,M:....::.....n,,z,;i;....z#:,+#:,::,,,:##xW:.#:,::,,,,,:;,,,,i#nnzznW#
                //                          ;Mx*,*zzxn+,,M..+W*:##..:Wz:::x+,:::,:::x*W,.*xnnnnnnx@W@Mxxxz;....#z`
                //                           `;MMzxzn#znn*..z*,z*;n.ziz;:::x+,#z:,xi;xz*..:i#####*;...........,W`
                //                            i+i,:,i*zn...:#Mx;x,*zx:#i:;,:z+:zi,:M:iMM......................++
                //                            z+zn:,+i,*xz;,zn:*x+;xn:iz:x:::x+:n;,;:,:M:.....................x:
                //                            z+nMz:iWz,;*nz;z.+izi#+::x:*z:::#i;n,::,:+*.....................M.
                //                           `@;x+Mz:*W+::::zzin+x:#::,zi:*:::::,z+,:nn+n.....................M.
                //                            +x+n++Mi;xWnzi,izi::;n:;,;n:::::,:::x;,:;+n.....................@.
                //                            `#@x+x*::,;**i#nz;iznz+n*:z+:**:,z::;xzi,;n....................:@.
                //                              `*M#xzi+i,:;i:z;:*n:M;z,:x;:x;,iz:,:i;:*n....................:#,
                //                               `z+,;zW#MMn@zW@@x:,zi;:,;x,in:*i+:*:::*z.....................@;
                //                                `WMxz#@Wix*##i:+::++,;;:;,:x;*+,:#xx;*+.....................n;
                //                                *x;iz@x+iz*::i:++:iz,++:::,in:n;,,:;x#+.....................#+
                //                                xMn##i:+M:z,:zi:,::x,:x,:;,:#*:x*,,::W+.....................*+
                //                               `M+:::n,;x::z::x;x::ni,x;:M:,:M::zx+:,zi.....................,x
                //                               +##++:n,:W,:x;:#+ix;;n,in,#+::*+::;+x,M,......................x`
                //                              ,x++++:::,x;:;x::M,ix:i::x::x:::n#::,#+W.......................*+
                //                              #*z*zi:x:,zi*i#i,n;,ni,:,++,:,,::*;,::*x........................M`
                //                             `M:x,M::x::x::x;x:in:in::::x:,::,:,:::iMz........................z:
                //                             ###*:*,:x::M,::Mix::,:M,+#:M,:::z::nxMn@,........................;n
                //                            `x:++,,::x:*z,M:;z+#:,,ni:#:ni,::i+::,,+x..........................M.
                //                             z+n#,,:+#:xi:#;,x;n:,:+#:,:iz::::ni:,;W#..........................#i
                //                             #Mi+z;;M:+z::**:#+x:,::z::,:#,,,:;ni#WM,..........................;n   ```
                //                             z+:,n;;;;M;i,:#::xx:::i+#:#i::::::iM#,......................;#znMMM@MMMWWW;
                //                             n;:,n;i:@i*#,:n,:xz*::x;n*;M::znnn@;.....................,#W@WWWWWWWWWWWWWW`
                //                            `xMi:xxzM#,;x,:x;:##x,:i*:zz*;:::*W;....................,z@WWWWWWWWWWWWWWWW@:
                //                            .#iM+MxMn:,##::*z:inxi:,:::*z:;#xn,....................#@WWWWWWWWWWWWWWWWWWWz ,+i
                //                            :+,+@i,:M,:x::,:x::Mix:,,z+:;Mz*,....................+WWWWWWWWWWWWWWWWWWWWWWWM@W@z`
                //                            **::::+,x::z**:,zi:#*iM*::nWM:.....................,x@WWWWWWWWWWWWWWWWWWWW@@WWWWWWx`
                //                            **x,:;n,#+,#*x::i+:;x::z**Wn*,...................;z@WWWWWWWWWWWW@@@WWWWW@@WWWWWWWWWM.
                //                            ,nx;,;x::i:*#n;::+::;;:ixz:.M,.................:n@WWWWWWWWWWW@@@WWWWW@@WWWWWWWWWWWWWM,
                //                             n+n,:M:,,,iz#*::,,:*,z@,...W,...............:z@WWWWWWWWWWW@@WWWWWWWWWWWWWWWWWWWWWWW@W+,
                //                             :n+*;z*,:,;xiz,z*::zn*W...:@,.............,#@WWWWWWWWWW@@@WWWWWWWWWWWWWWWWWWWWWWW@@@WWWx+,
                //                              iziz;x,:,,i::::n#xx#`x,..*n............:z@WWWWWWWWWW@@WWWWWWWWWWWWWWWWWWWWWW@@@@WWWWWWWW@Mi`
                //                               zxxi::+,,;x;,,n#.`  ;#..z;..........;n@WWWWWWWWWW@@WWWWWWWWWWWWWWWWWW@@@@@@WWWWWWWWWWWWWWWW:
                //                               `+@M;:M;,:ix:#+     +W..#........,+M@WWWWWWWWWW@@WWWWWWWWWWWWWWWW@@@@WWWWWWWWWWWWWWWWWWW@Wz,
                //                                 `ixzx@zznxz+     ;WW#.......:+x@WWWWWWWWWW@@@WWWWWWWWWWWW@@@@@WWWWWWWWWWWWWWWWWWWWWMn#i`
                //                                   `,,ii.,,      +@WWz..,;#xW@WWWWWWWWWW@#@WWWWWWWWWWW@@@@WWWWWWWWWWWWWWWWWWWWWW@Wn;`
                //                                                i#WWWWnx@@WWWWWWWWWW@@@@WWWWWWWWWWWW@@WWWWWWWWWWWWWWWWWWWWWWW@x+,`
                //                                                x@WWWWWWWWWWWWW@@@@@WWWWWWWWWWW@@@@@WWWWWWWWWWWWWWWWWWWWW@Wzi.
                //                                               ,@WWWWWWWWWW@@@WWWWWWWWWWWWWW@@@WWWWWWWWWWWWWWWWWWW@WMn#ii:.
                //                                              `MWWWWWWWW@@@WWWWWWWWWWWWWW@@@WWWWWWWWWWWWWWWWWWW@W+,
                //                                              *@WWWWW@@@WWWWWWWWWWWWWWW@@WWWWWWWWWWWWWWWWWWW@W#:`
                //                                             `MWWWW@@WWWWWWWWWWWWWWW@@@WWWWWWWWWWWWWWWWWWWWz:`
                //                                             ,@WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW@@x*`
                //                                             *@WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW@@xi.
                //                                             xWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW@Wn;.
                //                                             MWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW@Mi`
                //                                             xWWWWWWWWWWWWWWWWWWWWWWWWWWWWW@W*,
                //                                             MWWWWWWWWWWWWWWWWWWWWWWWWWWMn+,
                //                                             MWWWWWWWWWWWWWWWWW@@@MMzi:`
                //                                             :nxx#iiiiiiiiiiiii:,.
                //
                //
                //
                //
                //
                //
                //
                //
                //                                                    ;MMMWMMMn   +MMMWMM`
                //                                                    `:n####,`   `,+##n:
                //                                                      `x##z       ,#z
                //                                                       ,@#@,      nM`
                //                                                        z##z     :@:
                //                                                        .@##,   `M#        `,:,`        ,:,``:
                //                                                         +##n   *@.      `+W@W@W+`    :M@W@@x@
                //                                                         `@##: .W*      `n##` ,W#z   ,@x. `*@#
                //                                                          *##M`zM`      +#n    ;##:  z#;    *#`
                //                                                          `W###@;      .@#;    `@##  M#*    `W`
                //                                                           *###x       *##`     W#x  x#@i`   `
                //                                                           `W##;       z##zzzzzz@#W  ;###W#:
                //                                                            x##,       x##++++++++*   *@####x,
                //                                                            x##,       n##`            .+W###@,
                //                                                            x##,       ###,         `.    :n##z
                //                                                            x##,       i##*       ` ,@`     z#M
                //                                                            x##,       `W#W`     :W`.#*     ;#x   +@x.
                //                                                           `W##i        i##n.   ,W+ .#@:    +#i  .###*
                //                                                         i+x###@n+.      +##@n#n@+  `#W@#::+@z   .@##i
                //                                                         +########.       ,zMWWz:   `#`ixWWni     ;n+`

                str = str.Remove(index);

            return str;
        }

        public void Write(int offset, byte value) {
            Write(offset, new[] {
                value
            });
        }

        public virtual void Write(int offset, byte[] data) {
            if (Disposed)
                throw new ObjectDisposedException(nameof(KernelDrivenMemoryBase));

            if (!Kernel.WriteProcessMemory(m_processHandle, offset, data, data.Length, out var written) || written != data.Length)
                throw new InvalidOperationException($"Can't write bytes at 0x{offset:X} (written {written}/{data.Length}; error: 0x{Kernel.GetLastError():X})");
        }

        public void Write<T>(int offset, T data) where T : new() {
            var bytes = new byte[Marshal.SizeOf<T>()];

            unsafe {
                fixed (byte* buffer = bytes) {
                    Marshal.StructureToPtr(data, (IntPtr)buffer, true);
                }
            }

            Write(offset, bytes);
        }

        public void Write<T>(int offset, T[] data) where T : new() {
            var size = Marshal.SizeOf<T>();
            var bytes = new byte[size * data.Length];

            unsafe {
                fixed (byte* buffer = bytes) {
                    for (var i = 0; i < data.Length; i++)
                        Unsafe.As<byte, T>(ref buffer[i * size]) = data[i];
                }
            }

            Write(offset, bytes);
        }

        public void Write(int offset, IAutoManagedMemory pointer) {
            Write(offset, pointer.Start);
        }

        public void WriteNullTerminatedString(int offset, string data, Encoding encoding) {
            Write(offset, encoding.GetBytes(data)
                                  .Append((byte)0)
                                  .ToArray());
        }

        protected override void Dispose(bool disposing) {
            Disposing(this, EventArgs.Empty);
            base.Dispose(disposing);
        }

        #endregion Methods
    }
}