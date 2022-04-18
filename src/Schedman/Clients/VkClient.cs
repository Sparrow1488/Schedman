using Polly;
using Polly.Retry;
using Schedman.Abstractions;
using System;
using System.Threading.Tasks;
using VkNet.Exception;

namespace Schedman.Clients
{
    internal class VkClient : IVkClient
    {
        public VkClient()
        {
            _retryPolicy = Policy.Handle<VkApiException>()
                                 .WaitAndRetryAsync(3, sleepTime => TimeSpan.FromSeconds(1));
        }

        private readonly AsyncRetryPolicy _retryPolicy;

        public async Task<TResult> SendRetryAsync<TResult>(VkCommandBase command)
            where TResult : class
        {
            await _retryPolicy.ExecuteAsync(() => command.ExecuteAsync());
            return command.GetResultOrDefault<TResult>();
        }

        public async Task SendRetryAsync(VkCommandBase command)
        {
            await _retryPolicy.ExecuteAsync(() => command.ExecuteAsync());
        }
    }
}
