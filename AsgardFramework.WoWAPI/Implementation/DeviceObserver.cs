using AsgardFramework.DirectXObserver;
using AsgardFramework.Memory;

namespace AsgardFramework.WoWAPI.Implementation
{
    internal class DeviceObserver : IIDirect3DDevice9Observer
    {
        #region Constructors

        internal DeviceObserver(IGlobalMemory memory) {
            var ppDevice = memory.Read<int>(c_dxDevice) + c_dxDeviceIdx;
            var pIDirect3DDevice9 = memory.Read<int>(ppDevice);
            pEndScene = memory.Read<int>(pIDirect3DDevice9) + c_endSceneIdx;
            EndScene = memory.Read<int>(pEndScene);
        }

        #endregion Constructors

        #region Fields

        private const int c_dxDevice = 0xC5DF88;  // ?
        private const int c_dxDeviceIdx = 0x397C; // **IDirect3DDevice9
        private const int c_endSceneIdx = 0xA8;

        #endregion Fields

        #region Properties

        public int EndScene { get; }

        public int pEndScene { get; }

        #endregion Properties
    }
}