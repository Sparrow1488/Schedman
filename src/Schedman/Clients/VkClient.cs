using Schedman.Abstractions;
using System.Threading.Tasks;

namespace Schedman.Clients
{
    internal class VkClient : IVkClient
    {
        public async Task<TResult> SendRetryAsync<TResult>(VkCommandBase command)
        {
            await command.ExecuteAsync();
            return command.GetResultOrDefault<TResult>();
        }

        public async Task SendRetryAsync(VkCommandBase command)
        {
            await command.ExecuteAsync();
        }
    }
}
