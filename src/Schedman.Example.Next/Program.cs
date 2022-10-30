using Schedman.Entities;
using Schedman.Example.Next.Services;
using Schedman.Tools.IO;
using Schedman.Tools.IO.Configurations;
using Schedman.Tools.IO.Services;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Schedman.Example.Next
{
    internal class Program
    {
        private static async Task Main()
        {
            var access = new AccessPermission(ConfigurationManager.AppSettings["accessFile"]);
            var manager = new VkManager();
            await manager.AuthorizateAsync(access);

            IService executableService = new DownloadVideosService(manager);
            await executableService.StartAsync();
        }
    }
}
