using AsgardFramework.DirectXObserver;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class DeviceObserver : IIDirect3DDevice9Observer
    {
        private const int DX_DEVICE = 0xC5DF88; // ?
        private const int DX_DEVICE_IDX = 0x397C; // **IDirect3DDevice9
        private const int ENDSCENE_IDX = 0xA8;
        private readonly int pIDirect3DDevice9;
        internal DeviceObserver(IGlobalMemory memory) {
            var ppDevice = memory.Read<int>(DX_DEVICE) + DX_DEVICE_IDX;
            pIDirect3DDevice9 = memory.Read<int>(ppDevice);
            pEndScene = memory.Read<int>(pIDirect3DDevice9) + ENDSCENE_IDX;
            EndScene = memory.Read<int>(pEndScene);
        }
        public int EndScene { get; private set; }

        public int pEndScene { get; private set; }
    }
}
