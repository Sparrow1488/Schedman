using ScheduleVkManager.ChatBot.Commands.Adapters;
using ScheduleVkManager.ChatBot.Entities;

namespace ScheduleVkManager.ChatBot.Services.Interfaces
{
    public interface IVkCommandsSelector
    {
        IVkCommandAdapter Select(VkCallback request);
    }
}
