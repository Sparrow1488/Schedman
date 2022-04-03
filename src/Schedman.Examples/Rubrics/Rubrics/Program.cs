using Schedman;
using Schedman.Entities;
using Schedman.Exceptions;
using Schedman.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace Rubrics
{
    internal sealed class Program
    {
        private static VkSchedmanTool _vkTools;
        private static OwnTools _vkOwnTools;
        private static UserInfo _myInfo;

        private static async Task Main()
        {
            _vkTools = SchedmanFactory.CreateVkTools(CreateAccessPermission());
            await CompleteAuthorizationAsync();
            if (_vkTools.IsAuthorized)
            {
                _vkOwnTools = _vkTools.GetOwnTools();
                _myInfo = await _vkOwnTools.GetInfoAsync();
                System.Console.WriteLine(_myInfo.Id + " " + _myInfo.Name);
            }
        }

        private static AccessPermission CreateAccessPermission()
        {
            var loginAndPassword = File.ReadAllLines(@".\access.auth");
            return new AccessPermission(loginAndPassword[0], loginAndPassword[1]);
        }
        
        private static async Task CompleteAuthorizationAsync()
        {
            try {
                await _vkTools.AuthorizeAsync();
            }
            catch (AuthorizationException ex) {
                throw ex;
            }
            catch {
                throw;
            }
        }
    }
}
