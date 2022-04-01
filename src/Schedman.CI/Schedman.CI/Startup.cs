using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Schedman.CI.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace Schedman.CI
{
    internal class Startup : IHostedService
    {
        public Startup(IAuthorizationService authorization, ILogger<Startup> logger)
        {
            _authorization = authorization;
            _logger = logger;
        }

        private readonly IAuthorizationService _authorization;
        private readonly ILogger<Startup> _logger;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hello Schedman.CI!");
            await _authorization.AuthorizeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
