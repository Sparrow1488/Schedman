using VkSchedman.ChatBot.Commands.Adapters;
using VkSchedman.ChatBot.Entities;
using VkSchedman.ChatBot.Services.Interfaces;
using VkNet.Model;
using VkNet.Utils;

namespace VkSchedman.ChatBot.Services
{
    public sealed class VkCommandsSelector : IVkCommandsSelector
    {
        public IVkCommandAdapter Select(VkCallback request)
        {
            IVkCommandAdapter resCommand = null;
            var userInput = Message.FromJson(new VkResponse(request.Object));
            string command = userInput.Text;
            if (command.Contains("/get")) {
                resCommand = new ToGetVkCommand();
            }
            return resCommand;
        }
    }
}
