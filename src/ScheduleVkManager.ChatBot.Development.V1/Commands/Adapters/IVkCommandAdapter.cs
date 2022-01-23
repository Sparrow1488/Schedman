using ScheduleVkManager.ChatBot.Entities;
using VkNet;
using VkNet.Abstractions;

namespace ScheduleVkManager.ChatBot.Commands.Adapters
{
    public interface IVkCommandAdapter : ICommandAdapter<VkCallback>
    {
        void UseApi(IVkApi api);
    }
}
