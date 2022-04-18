using System.Threading.Tasks;

namespace Schedman.Abstractions
{
    internal interface IVkClient
    {
        Task<TResult> SendRetryAsync<TResult>(VkCommandBase command)
            where TResult : class;
        Task SendRetryAsync(VkCommandBase command);
    }
}
