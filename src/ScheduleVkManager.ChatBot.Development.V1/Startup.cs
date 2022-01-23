using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduleVkManager.ChatBot.Entities;
using ScheduleVkManager.ChatBot.Services;
using ScheduleVkManager.ChatBot.Services.Interfaces;
using Serilog;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;

namespace ScheduleVkManager.ChatBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();
            services.AddSingleton<IVkApi>(sp => {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams { AccessToken = Configuration["VkConfig:AccessToken"] });
                if (api.IsAuthorized)
                {
                    Log.Information("Authorizated on vk was success");
                }
                else Log.Error("Authorizated on vk was failed");
                return api;
            });
            services.AddSingleton<BotSettings>();
            services.AddSingleton<IVkCommandsHandler, VkCommandsHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
