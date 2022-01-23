using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ScheduleVkManager.ChatBot.Entities;
using ScheduleVkManager.ChatBot.Services;
using ScheduleVkManager.ChatBot.Services.Interfaces;
using Serilog;
using System;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace ScheduleVkManager.ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        public CallbackController(
            IVkApi api, IConfiguration configuration, 
                BotSettings settings, IVkCommandsHandler commandsHandler) {
            _api = api;
            _config = configuration;
            _settings = settings;
            _commandsHandler = commandsHandler;
        }

        private readonly IVkApi _api;
        private readonly IConfiguration _config;
        private readonly BotSettings _settings;
        private readonly IVkCommandsHandler _commandsHandler;

        [HttpPost]
        public IActionResult Callback([FromBody] VkCallback vkRequest)
        {
            IActionResult response = Ok("ok");

            try {
                if (vkRequest.Type.ToLower().Contains("confirmation")) {
                    Log.Information("Get confirmation");
                    response = Ok(_config["VkConfig:Confirmation"]);
                }
                else if (vkRequest.Type.ToLower().Contains("message_new")) {
                    if (!_settings.Pause) {
                        var commandAdapter = _commandsHandler.Handle(vkRequest);
                        commandAdapter.UseApi(_api);
                        var userInput = Message.FromJson(new VkResponse(vkRequest.Object));
                        string command = userInput.Text;
                        var result = commandAdapter.Execute(command, vkRequest);
                    }
                }
            } catch (Exception ex) {
                Log.Error("UnhandleException: " + ex.Message);
                response = Ok(new {
                    error = ex.Message,
                    status = "bad"
                });
            }
            return response;
        }

        [HttpPost("settings")]
        public IActionResult Settings([FromBody] BotSettings settings)
        {
            _settings.Pause = settings.Pause;
            Log.Information("Set pause mode: " + _settings.Pause);
            return Ok("ok");
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return new JsonResult(new {
                status = "ok",
                isAuth = _api.IsAuthorized
            });
        }
    }
}
