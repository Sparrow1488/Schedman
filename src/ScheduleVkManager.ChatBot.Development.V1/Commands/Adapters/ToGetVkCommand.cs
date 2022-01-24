using ScheduleVkManager.ChatBot.Entities;
using System;
using System.IO;
using System.Linq;
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
                string commandTarget = chaps[1];
                if (commandTarget == "group_name") {
                    response = WriteGroupName(input);
                }
                if (commandTarget == "help") {
                    response = WriteCommandsInfo(input);
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
            var message = Message.FromJson(new VkResponse(input.Object));
            var groupName = _vkApi.Groups.GetById(null, input.GroupId.ToString(), null)
                                         ?.FirstOrDefault()?.Name ?? string.Empty;

            string commandResult = $"Вы находитесь в сообществе {groupName}! Не забывайте об этом";
            _vkApi.Messages.Send(new MessagesSendParams() {
                RandomId = DateTime.Now.Millisecond,
                PeerId = message.PeerId.Value,
                Message = commandResult
            });

            var response = new CommandResult(commandResult);
            return response;
        }

        private CommandResult WriteCommandsInfo(VkCallback input)
        {
            var message = Message.FromJson(new VkResponse(input.Object));
            string commandResult = $"Другалек, присаживайся, заваривай шаван и в путь изучать команды бота:\n";
            commandResult += File.ReadAllText("BotCommands.txt");
            _vkApi.Messages.Send(new MessagesSendParams()
            {
                RandomId = DateTime.Now.Millisecond,
                PeerId = message.PeerId.Value,
                Message = commandResult
            });

            var response = new CommandResult(commandResult);
            return response;
        }
    }
}
