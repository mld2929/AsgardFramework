using System.Threading.Tasks;

namespace AsgardFramework.CodeInject
{
    public interface ICodeExecutor
    {
        Task<int> Execute(ICodeBlock code);
    }
}
