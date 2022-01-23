using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ScheduleVkManager.ChatBot.Entities;
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
        public CallbackController(IVkApi api, IConfiguration configuration, BotSettings settings) {
            _api = api;
            _config = configuration;
            _settings = settings;
        }

        private readonly IVkApi _api;
        private readonly IConfiguration _config;
        private readonly BotSettings _settings;

        [HttpPost]
        public IActionResult Callback([FromBody] VkCallback vkRequest)
        {
            Log.Information("Callback method invoked");
            IActionResult response = Ok("ok");

            try {
                if (vkRequest.Type.ToLower().Contains("confirmation")) {
                    response = Ok(_config["VkConfig:Confirmation"]);
                }
                else if (vkRequest.Type.ToLower().Contains("message_new")) {
                    if (!_settings.Pause) {
                        var message = Message.FromJson(new VkResponse(vkRequest.Object));
                        var messageId = _api.Messages.Send(new MessagesSendParams() {
                            RandomId = DateTime.Now.Millisecond,
                            PeerId = message.PeerId.Value,
                            Message = "Вечер в хату, " + message.PeerId
                        });
                    }
                }
            } catch (Exception ex) {
                response = Ok(new {
                    error = ex.Message,
                    status = "bad"
                });
            }
            return response;
        }

        [HttpGet("pause")]
        public IActionResult SetPause()
        {
            _settings.Pause = !_settings.Pause;
            Log.Information("Set pause mode: " + _settings.Pause);
            return Ok(_settings.Pause);
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            if (_api.IsAuthorized)
            {
                Log.Warning("Was authorizated on vk");
            }
            return new JsonResult(new {
                status = "ok",
                isAuth = _api.IsAuthorized
            });
        }
    }
}
