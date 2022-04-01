using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Schedman.CI.Abstractions;
using Schedman.CI.Commands;
using System.IO;
using System.Threading.Tasks;

namespace Schedman.CI.Services
{
    internal class AuthorizationService : IAuthorizationService
    {
        public AuthorizationService(IConfiguration config, ILogger<AuthorizationService> logger)
        {
            _authCommand = new AuthorizationCommand();
            _config = config;
            _logger = logger;
        }

        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizationService> _logger;
        private readonly ICommand _authCommand;

        public async Task AuthorizeAsync()
        {
            string filePath = GetAuthFilePathFromConfig();
            _logger.LogInformation("Take auth from: " + filePath);
            string[] auth = await ReadAuthDataFromFileAsync(filePath);
            _logger.LogDebug($"Login => {auth[0]}; Password => {auth[1]}");
            await _authCommand.ExecuteAsync();
        }

        private string GetAuthFilePathFromConfig() =>
            _config.GetSection("Authorization").GetSection("Vk").GetValue<string>("From");

        private async Task<string[]> ReadAuthDataFromFileAsync(string path) =>
            (await File.ReadAllTextAsync(path)).Split("\n");
    }
}
