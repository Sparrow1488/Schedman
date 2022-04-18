using Schedman.Abstractions;
using Schedman.Entities;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;

namespace Schedman.Commands
{
    internal class VkAuthorizationCommand : VkCommandBase
    {
        public VkAuthorizationCommand(IVkApi api, AccessPermission access) : base(api)
        {
            _access = access;
        }

        private readonly AccessPermission _access;

        public override async Task ExecuteAsync()
        {
            await Api.AuthorizeAsync(new ApiAuthParams
            {
                Login = _access.Login,
                Password = _access.Password,
            });
        }

        public override TResult GetResultOrDefault<TResult>()
        {
            return default;
        }
    }
}
