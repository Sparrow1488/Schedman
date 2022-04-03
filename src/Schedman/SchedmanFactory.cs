using Microsoft.Extensions.DependencyInjection;
using Schedman.Entities;
using VkNet;
using VkNet.AudioBypassService.Extensions;

namespace Schedman
{
    public static class SchedmanFactory
    {
        public static VkSchedmanTool CreateVkTools(AccessPermission access)
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            var vkApi = new VkApi(services);
            return new VkSchedmanTool(access, vkApi);
        }
    }
}
