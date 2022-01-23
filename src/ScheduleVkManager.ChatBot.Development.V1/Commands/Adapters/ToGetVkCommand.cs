using ScheduleVkManager.ChatBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace ScheduleVkManager.ChatBot.Commands.Adapters
{
    public class ToGetVkCommand : IVkCommandAdapter
    {
        private IVkApi _vkApi;

        public CommandResult Execute(string command, VkCallback input)
        {
            CommandResult response = new CommandResult();
            var chaps = command.Split(" ");
            if(chaps.Length >= 2) {
                if (chaps[1] == "group_name") {
                    response = WriteGroupName(input);
                }
            }
            return response;  
        }

        public void UseApi(IVkApi api)
        {
            _vkApi = api;
        }

        private CommandResult WriteGroupName(VkCallback input)
        {
            var response = new CommandResult();
            var message = Message.FromJson(new VkResponse(input.Object));
            var groups = _vkApi.Groups.GetById(new List<string>() { 
                _vkApi.UserId.ToString()
            }, _vkApi.UserId.ToString(), null);
            var groupName = groups.FirstOrDefault().Name;
            _vkApi.Messages.Send(new MessagesSendParams()
            {
                RandomId = DateTime.Now.Millisecond,
                PeerId = message.PeerId.Value,
                Message = "Название группы: " + groupName
            });

            return response;
        }
    }
}
