using Microsoft.Extensions.Configuration;
using Serilog;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VkNet.Exception;
using VkSchedman.Entities;
using VkSchedman.Examples.Services;
using VkSchedman.Extensions;
using VkSchedman.Tools;

namespace VkSchedman.Examples
{
    public sealed class Program
    {
        public static async Task Main()
        {
            var builder = new ConfigurationBuilder();
            InitConfiguration(builder);
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .ReadFrom.Configuration(builder.Build())
                         .WriteTo.Console()
                         .WriteTo.File("logging.txt")
                         .CreateLogger();

            Console.Title = "VkSchedman [not authorizated]";
            AnsiConsole.Write(new FigletText("VkSchedman").Color(Color.SkyBlue1).LeftAligned());
            Log.Information("Started VkSchedman");
            
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
                Log.Fatal(ex, ex.Message);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static async Task AuthorizeManagerAsync(VkManager manager, AuthorizeData authData)
        {
            int attempts = 3;
            do
            {
                Log.Information("Try authorize...");
                try
                {
                    await manager.AuthorizeAsync(authData);
                }
                catch (VkAuthorizationException)
                {
                    Log.Error("Authorize failed");
                }
                if (manager.IsAuthorizated)
                    attempts = -1;
                attempts--;
            }
            while (attempts > 0);

            manager.ThrowIfNotAuth();
            Log.Information("Authorize success");
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
                authDataPath = new AuthorizeData(AnsiConsole.Ask("[blue]Login[/]: ", string.Empty), 
                                                 AnsiConsole.Ask("[blue]Password[/]: ", string.Empty));
            return authDataPath;
        }

        private static void InitConfiguration(IConfigurationBuilder builder) =>
           builder.SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    }
}
