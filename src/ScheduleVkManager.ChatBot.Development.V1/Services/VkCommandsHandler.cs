using ScheduleVkManager.ChatBot.Commands.Adapters;
using ScheduleVkManager.ChatBot.Entities;
using ScheduleVkManager.ChatBot.Services.Interfaces;
using System;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace ScheduleVkManager.ChatBot.Services
{
    public sealed class VkCommandsHandler : IVkCommandsHandler
    {
        public IVkCommandAdapter Handle(VkCallback request)
        {
            IVkCommandAdapter resCommand = null;
            var userInput = Message.FromJson(new VkResponse(request.Object));
            string command = userInput.Text;
            if (command.Contains("/get"))
            {
                resCommand = new ToGetVkCommand();
            }
            return resCommand;
        }
    }
}
