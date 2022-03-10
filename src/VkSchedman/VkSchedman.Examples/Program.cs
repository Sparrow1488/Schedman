using Spectre.Console;
using System;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Entities;
using VkSchedman.Examples.Entities;
using VkSchedman.Examples.Services;
using VkSchedman.Extensions;
using VkLogger = VkSchedman.Logging.Logger;

namespace VkSchedman.Examples
{
    public sealed class Program
    {
        public static async Task Main()
        {
            InitLoggers();
            Console.Title = "VkSchedman [not authorizated]";
            AnsiConsole.Write(new FigletText("VkSchedman").Color(Color.Purple3).LeftAligned());
            Logger.Info("Started VkSchedman");
            
            try
            {
                var authData = CreateAuthorizationData();
                var vkManager = new VkManager();
                await AuthorizeManagerAsync(vkManager, authData);

                string[] services = new[] { "Download videos", "Autoposter" };
                var choose = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                    .Title("Select [green]service[/] to use")
                                    .AddChoices(services));

                if (choose == "Download videos")
                    await new VideosHandleService(vkManager).StartAsync();
                if (choose == "Autoposter")
                    await new VkSchedulerService(vkManager).StartAsync();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex);
            }
        }

        private static async Task AuthorizeManagerAsync(VkManager manager, AuthorizeData authData)
        {
            int attempts = 3;
            do
            {
                Logger.Info("Try authorize...");
                try
                {
                    await manager.AuthorizeAsync(authData);
                }
                catch (VkAuthorizationException)
                {
                    Logger.Info("Authorize failed");
                }
                if (manager.IsAuthorizated)
                    attempts = -1;
                attempts--;
            }
            while (attempts > 0);

            manager.ThrowIfNotAuth();
            Logger.Info("Authorize success");
            Console.Title = "VkSchedman";
        }

        private static AuthorizeData CreateAuthorizationData()
        {
            var authTypes = new[] { "Configuration", "Login&pass" };
            var authType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                                .Title("Select [purple]authorization way[/]")
                                .AddChoices(authTypes));
            AuthorizeData authDataPath = null;
            if (authType == "Configuration")
                authDataPath = new AuthorizeData(System.Configuration.ConfigurationManager.AppSettings["Auth"]);
            else if (authType == "Login&pass")
                authDataPath = new AuthorizeData(AnsiConsole.Ask("Login: ", string.Empty), 
                                                 AnsiConsole.Ask("Password: ", string.Empty));
            return authDataPath;
        }

        private static void InitLoggers()
        {
            VkLogger.SetDebugLogging(message => AnsiConsole.WriteLine($"[Debug] " + message));
            VkLogger.SetInfoLogging(message => AnsiConsole.WriteLine($"[Info] " + message));
            VkLogger.SetErrorLogging(message => AnsiConsole.WriteLine($"[Error] " + message));

            Logger.SetDebugLogging(message => AnsiConsole.WriteLine($"[Debug] " + message));
            Logger.SetInfoLogging(message => AnsiConsole.WriteLine($"[Info] " + message));
            Logger.SetErrorLogging(message => AnsiConsole.WriteLine($"[Error] " + message));
            Logger.SetExceptionLogging(ex => AnsiConsole.WriteException(ex));
        }
    }
}
