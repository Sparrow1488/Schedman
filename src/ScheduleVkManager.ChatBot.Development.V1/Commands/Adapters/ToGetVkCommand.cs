using ScheduleVkManager.ChatBot.Entities;
using System.IO;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace ScheduleVkManager.ChatBot.Commands.Adapters
{
    public class ToGetVkCommand : IVkCommandAdapter
    {
        private IVkApi _vkApi;

        public CommandResult Execute(string command, VkCallback input)
        {
            CommandResult response = null;
            var chaps = command.Split(" ");
            if(chaps.Length >= 2) {
                string commandTarget = chaps[1];
                if (commandTarget == "group_name") {
                    response = GetGroupName(input);
                }
                if (commandTarget == "help") {
                    response = GetCommandsInfo(input);
                }
            }
            return response ?? new CommandResult($"Не найдена цель команды \"{command}\"",
                                                    dialog: Message.FromJson(new VkResponse(input.Object)).PeerId.Value);  
        }

        public void UseApi(IVkApi api)
        {
            _vkApi = api;
        }

        private CommandResult GetGroupName(VkCallback input)
        {
            var message = Message.FromJson(new VkResponse(input.Object));
            var groupName = _vkApi.Groups.GetById(null, input.GroupId.ToString(), null)
                                         ?.FirstOrDefault()?.Name ?? string.Empty;

            return new CommandResult($"Вы находитесь в сообществе {groupName}! Не забывайте об этом",
                                        dialog: message.PeerId.Value);
        }

        private CommandResult GetCommandsInfo(VkCallback input)
        {
            var message = Message.FromJson(new VkResponse(input.Object));
            string commandResult = $"Другалек, присаживайся, заваривай шаван и в путь изучать команды бота:\n";
            commandResult += File.ReadAllText("BotCommands.txt");

            return new CommandResult(commandResult, dialog: message.PeerId.Value);
        }
    }
}
