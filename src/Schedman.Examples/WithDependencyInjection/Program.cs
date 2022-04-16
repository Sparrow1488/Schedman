using Schedman;
using Schedman.Entities;
using Schedman.Extensions;
using Spectre.Console;
using System;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Examples.Services;

namespace VkSchedman.Examples
{
    public sealed class Program
    {
        public static async Task Main()
        {
            Console.Title = "VkSchedman [not authorizated]";
            AnsiConsole.Write(new FigletText("VkSchedman").Color(Color.Purple3).LeftAligned());
            
            try
            {
                bool isWorking = true;
                while (isWorking)
                {
                    var authData = CreateAuthorizationData();
                    var vkManager = new VkManager();
                    await AuthorizeManagerAsync(vkManager, authData);

                    string[] services = new[] { "Download videos", "Autoposter", "Exit" };
                    var choose = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                        .Title("Select [green]service[/] to use")
                                        .AddChoices(services));

                    if (choose == "Download videos")
                        await new VideosHandleService(vkManager).StartAsync();
                    if (choose == "Autoposter")
                        await new VkSchedulerService(vkManager).StartAsync();
                    if(choose == "Exit")
                        isWorking = false;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private static async Task AuthorizeManagerAsync(VkManager manager, AccessPermission authData)
        {
            int attempts = 3;
            do
            {
                try
                {
                    await manager.AuthorizateAsync(authData);
                }
                catch (VkAuthorizationException)
                {
                }
                if (manager.IsAuthorizated)
                    attempts = -1;
                attempts--;
            }
            while (attempts > 0);

            manager.ThrowIfNotAuth();
            Console.Title = "VkSchedman";
        }

        private static AccessPermission CreateAuthorizationData()
        {
            var authTypes = new[] { "Configuration", "Login&pass" };
            var authType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [purple]authorization way[/]")
                                .AddChoices(authTypes));
            AccessPermission authDataPath = null;
            if (authType == "Configuration")
                authDataPath = new AccessPermission(System.Configuration.ConfigurationManager.AppSettings["Auth"]);
            else if (authType == "Login&pass")
                authDataPath = new AccessPermission(AnsiConsole.Ask("Login: ", string.Empty), 
                                                 AnsiConsole.Ask("Password: ", string.Empty));
            return authDataPath;
        }
    }
}
