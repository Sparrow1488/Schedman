using ScheduleVkManager.ChatBot.Commands.Adapters;
using ScheduleVkManager.ChatBot.Entities;

namespace ScheduleVkManager.ChatBot.Services.Interfaces
{
    public interface IVkCommandsHandler
    {
        IVkCommandAdapter Handle(VkCallback request);
    }
}
