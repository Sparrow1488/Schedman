using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Schedman.CI.Abstractions;
using Schedman.CI.Services;
using Serilog;
using System.Threading.Tasks;

namespace Schedman.CI
{
    internal class Program
    {
        private static async Task Main() =>
            await Host.CreateDefaultBuilder()
                .ConfigureServices(services => ConfigureServices(services))
                    .UseSerilog((hostingContext, services, loggerConfiguration) => ConfigureLogger(loggerConfiguration))
                        .Build().StartAsync();

        private static IServiceCollection ConfigureServices(IServiceCollection services) =>
            services.AddHostedService<Startup>()
                    .AddTransient<IAuthorizationService, AuthorizationService>();

        private static LoggerConfiguration ConfigureLogger(LoggerConfiguration config) =>
            config.MinimumLevel.Debug()
                .Enrich.FromLogContext()
                    .WriteTo.Console();
    }
}
