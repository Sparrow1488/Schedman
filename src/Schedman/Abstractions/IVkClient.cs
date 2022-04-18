using System.Threading.Tasks;

namespace Schedman.Abstractions
{
    internal interface IVkClient
    {
        Task<TResult> SendRetryAsync<TResult>(VkCommandBase command);
        Task SendRetryAsync(VkCommandBase command);
    }
}
