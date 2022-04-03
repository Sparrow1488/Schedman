using Microsoft.Extensions.Logging;
using Schedman.Interfaces;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace Schedman.Entities
{
    public class VkOwnTools : OwnTools
    {
        public VkOwnTools(IVkApi api, ILogger logger)
        {
            _vkApi = api;
            _logger = logger;
        }

        private readonly IVkApi _vkApi;
        private readonly ILogger _logger;

        public override async Task<UserInfo> GetInfoAsync()
        {
            _logger?.LogDebug("Get own info...");
            var accountInfo = await _vkApi.Account.GetProfileInfoAsync();
            _logger?.LogDebug("Own info get success");
            return CreateUserInfo(accountInfo);
        }

        private UserInfo CreateUserInfo(AccountSaveProfileInfoParams profileInfo)
        {
            return new UserInfo() {
                Id = _vkApi.UserId ?? -1,
                Name = profileInfo.ScreenName
            };
        }
    }
}
