using System.Threading.Tasks;

namespace Schedman.CI.Abstractions
{
    internal interface ICommand
    {
        Task ExecuteAsync();
        Task UnexecuteAsync();
    }
}
