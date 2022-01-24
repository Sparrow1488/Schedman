using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ScheduleVkManager.ChatBot.Commands.Adapters;
using ScheduleVkManager.ChatBot.Entities;
using ScheduleVkManager.ChatBot.Services.Interfaces;
using Serilog;
using System;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace ScheduleVkManager.ChatBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {
        public CallbackController(
            IVkApi api, IConfiguration configuration, IVkCommandsSelector commandsHandler,
                IWritableService writable) {
            _api = api;
            _config = configuration;
            _commandsHandler = commandsHandler;
            _writable = writable;
        }

        private readonly IVkApi _api;
        private readonly IConfiguration _config;
        private readonly IVkCommandsSelector _commandsHandler;
        private readonly IWritableService _writable;

        [HttpPost]
        public IActionResult Callback([FromBody] VkCallback vkRequest)
        {
            IActionResult response = Ok("ok");

            try 
            {
                if (vkRequest.Type.ToLower().Contains("confirmation")) {
                    Log.Information("Get confirmation");
                    response = Ok(_config["VkConfig:Confirmation"]);
                }
                else if (vkRequest.Type.ToLower().Contains("message_new")) {
                    if (!_config.GetValue<bool>("BotConfig:Settings:Pause")) {
                        var commandAdapter = _commandsHandler.Select(vkRequest) ?? new EmptyVkCommand();
                        var userInput = Message.FromJson(new VkResponse(vkRequest.Object));

                        commandAdapter.UseApi(_api);
                        var result = commandAdapter.Execute(userInput.Text, vkRequest);
                        _writable.Write(result);
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

        [HttpGet("pause")]
        public IActionResult Pause()
        {
            string sectionName = "BotConfig:Settings:Pause";
            _config[sectionName] = (!_config.GetValue<bool>(sectionName)).ToString();
            Log.Information("Set pause mode: " + _config[sectionName]);
            return Ok(_config[sectionName]);
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return new JsonResult(new {
                status = "ok",
                isAuth = _api.IsAuthorized,
                isPaused = _config["BotConfig:Settings:Pause"],
                botVersion = _config["BotConfig:Version"]
            });
        }
    }
}
