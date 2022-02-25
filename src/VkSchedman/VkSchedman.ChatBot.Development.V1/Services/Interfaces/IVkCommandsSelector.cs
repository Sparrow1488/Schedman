using VkSchedman.ChatBot.Commands.Adapters;
using VkSchedman.ChatBot.Entities;

namespace VkSchedman.ChatBot.Services.Interfaces
{
    public interface IVkCommandsSelector
    {
        IVkCommandAdapter Select(VkCallback request);
    }
}
