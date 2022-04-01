using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Schedman.CI.Abstractions;
using System.Threading.Tasks;

namespace Schedman.CI.Services
{
    internal class AuthorizationService : IAuthorizationService
    {
        public AuthorizationService(IConfiguration config, ILogger<AuthorizationService> logger)
        {
            _config = config;
            _logger = logger;
        }

        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizationService> _logger;

        public async Task AuthorizeAsync()
        {
            _logger.LogInformation("Try authorize...");
            await Task.Delay(250);
            _logger.LogInformation("Authorize success");
        }
    }
}
