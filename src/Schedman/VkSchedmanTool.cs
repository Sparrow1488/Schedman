using Microsoft.Extensions.Logging;
using Schedman.Entities;
using Schedman.Interfaces;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Exception;
using VkNet.Model;

namespace Schedman
{
    public sealed class VkSchedmanTool : SchedmanTool
    {
        public VkSchedmanTool(AccessPermission access, IVkApi api, ILogger logger = default)
        {
            _access = access;
            _logger = logger;
            _api = api;
        }

        private readonly AccessPermission _access;
        private readonly ILogger _logger;
        private readonly IVkApi _api;

        public override bool IsAuthorized => _api.IsAuthorized;

        public override async Task AuthorizeAsync()
        {
            try
            {
                _logger?.LogDebug("Authorization started...");
                await ExecuteAuthorizationAsync();
            }
            catch (VkAuthorizationException ex)
            {
                _logger?.LogError("Authorization failed: " + ex.Message);
            }
            finally
            {
                if (IsAuthorized)
                    _logger?.LogInformation("Authorization completed success");
                else _logger?.LogError("Authorization failed");
            }
        }

        private async Task ExecuteAuthorizationAsync()
        {
            await _api.AuthorizeAsync(CreateApiAuthParams());
        }

        private IApiAuthParams CreateApiAuthParams()
        {
            return new ApiAuthParams()
            {
                Login = _access.Login,
                Password = _access.Password,
            };
        }

        public override OwnTools GetOwnTools() =>
            new VkOwnTools(_api, _logger);
    }
}
