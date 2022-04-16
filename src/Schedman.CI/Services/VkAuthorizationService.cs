using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Schedman.CI.Abstractions;
using Schedman.Entities;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Schedman.CI.Services
{
    internal class VkAuthorizationService : IAuthorizationService
    {
        public VkAuthorizationService(
            IConfiguration config, 
            ILogger<VkAuthorizationService> logger,
            VkManager vkManager)
        {
            _config = config;
            _logger = logger;
            _vkManager = vkManager;
        }

        private readonly IConfiguration _config;
        private readonly ILogger<VkAuthorizationService> _logger;
        private readonly VkManager _vkManager;

        public async Task AuthorizeAsync()
        {
            string filePath = GetAuthFilePathFromConfig();
            _logger.LogInformation("AuthorizeData FROM FILE => " + filePath);
            string[] auth = await ReadAuthDataFromFileAsync(filePath);
            _logger.LogDebug($"Login => {auth[0].Replace("\r", "")}; Password => {auth[1]}");

            await _vkManager.AuthorizeAsync(new AccessPermission(filePath));
            if (_vkManager.IsAuthorizated)
            {
                _logger.LogInformation("Authorize success");
            }
            else
            {
                _logger.LogError("Authorize failed");
            }
        }

        private string GetAuthFilePathFromConfig() =>
            _config.GetSection("Authorization").GetSection("Vk").GetValue<string>("From");

        private async Task<string[]> ReadAuthDataFromFileAsync(string path) =>
            (await File.ReadAllTextAsync(path, Encoding.UTF8))
                .Split("\n");
    }
}
