using ScheduleVkManager.ChatBot.Entities;
using System;
using System.Linq;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace ScheduleVkManager.ChatBot.Commands.Adapters
{
    public class EmptyVkCommand : IVkCommandAdapter
    {
        private IVkApi _vkApi;
        public CommandResult Execute(string command, VkCallback input)
        {
            var result = new CommandResult("Команда не была распознана");
            if (command.Length > 1)
            {
                bool isSlashCommandChar = command.FirstOrDefault() == '/' ? true : false;
                if (isSlashCommandChar) {
                    result = IncorrectInputCommandHandle(input);
                }
            }
            return result;
        }

        public void UseApi(IVkApi api)
        {
            _vkApi = api;
        }

        private CommandResult IncorrectInputCommandHandle(VkCallback input)
        {
            var message = Message.FromJson(new VkResponse(input.Object));
            string commandResult = $"Команда \"{message.Text}\" не была распознана. Попробуйте \"/get help\"";
            return new CommandResult(commandResult, dialog: message.PeerId.Value);
        }
    }
}
