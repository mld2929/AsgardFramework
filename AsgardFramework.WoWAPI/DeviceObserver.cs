using AsgardFramework.DirectXObserver;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI
{
    internal class DeviceObserver : IDirect3DDevice9Observer
    {
        private const int DX_DEVICE = 0xC5DF88; // ?
        private const int DX_DEVICE_IDX = 0x397C; // **IDirect3DDevice9
        private const int ENDSCENE_IDX = 0xA8;
        private readonly int pIDirect3DDevice9;
        internal DeviceObserver(IGlobalMemory memory)
        {
            var ppDevice = memory.Read(DX_DEVICE, 4).ToInt32() + DX_DEVICE_IDX;
            pIDirect3DDevice9 = memory.Read(ppDevice, 4).ToInt32();
            pEndScene = memory.Read(pIDirect3DDevice9, 4).ToInt32() + ENDSCENE_IDX;
            EndScene = memory.Read(pEndScene, 4).ToInt32();
        }
        public int EndScene { get; private set; }

        public int pEndScene { get; private set; }
    }
}
