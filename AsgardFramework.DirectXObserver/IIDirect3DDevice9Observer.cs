namespace AsgardFramework.DirectXObserver
{
    public interface IIDirect3DDevice9Observer
    {
        #region Properties

        /// <summary>
        ///     HRESULT (*EndScene)()
        /// </summary>
        int EndScene { get; }

        /// <summary>
        ///     *<see cref="EndScene" />
        /// </summary>
        int pEndScene { get; }

        #endregion Properties
    }
}