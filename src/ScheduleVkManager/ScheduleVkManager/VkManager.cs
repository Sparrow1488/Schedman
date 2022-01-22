using Microsoft.Extensions.DependencyInjection;
using ScheduleVkManager.Entities;
using ScheduleVkManager.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService.Exceptions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace ScheduleVkManager
{
    public class VkManager : IStorableErrors
    {
        public VkManager()
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            _api = new VkApi(services);
            Errors = new List<string>();
        }

        private readonly VkApi _api;
        public IList<string> Errors { get; private set; }

        public async Task<bool> AuthorizeAsync(AuthorizeData authorizeData)
        {
            bool authSuccess = false;
            var emptyContext = new ValidationContext(authorizeData);
            var validationErrors = authorizeData.Validate(emptyContext);

            if(validationErrors.Count() > 0) {
                foreach (var error in validationErrors) {
                    Errors.Add(error.ErrorMessage);
                }
            }
            else {
                authSuccess = await TryAuthorizeAsync(_api, authorizeData);
            }

            return authSuccess;
        }

        private async Task<bool> TryAuthorizeAsync(VkApi api, AuthorizeData authorizeData)
        {
            bool resultSuccess = true;
            try {
                await api.AuthorizeAsync(new ApiAuthParams {
                    Login = authorizeData.Login,
                    Password = authorizeData.Password,
                });
            }
            catch (VkAuthException authError) {
                Errors.Add(authError.Message);
            }
            catch {
                resultSuccess = false;
            }

            return resultSuccess;
        }

        public async Task<GroupManager> GetGroupManagerAsync(string groupName)
        {
            var userGroups = await _api.Groups.GetAsync(new GroupsGetParams() {
                UserId = _api.UserId,
            });
            var groupsId = new List<string>(userGroups.Where(gr => gr.Id != 0)
                                                        .Select(gr => gr.Id.ToString()));
            var groups = await _api.Groups.GetByIdAsync(groupsId, "", new GroupsFields());
            var foundGroup = groups?.Where(group => group.Name.ToLower().Contains(groupName.ToLower()))?.FirstOrDefault();
            
            if(foundGroup == null) {
                Errors.Add("Connot found group named " + groupName);
            }
            return new GroupManager(_api, foundGroup?.Id ?? 0);
        }

        public void ClearErrors() => Errors = new List<string>();
        public IList<string> GetErrors() => Errors;
    }
}
