using System.Threading.Tasks;

namespace AsgardFramework.CodeInject
{
    public interface ICodeExecutor
    {
        #region Methods

        Task<int> ExecuteAsync(ICodeBlock code);

        #endregion Methods
    }
}