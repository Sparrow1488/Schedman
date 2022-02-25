using VkSchedman.ChatBot.Entities;
using VkNet;
using VkNet.Abstractions;

namespace VkSchedman.ChatBot.Commands.Adapters
{
    public interface IVkCommandAdapter : ICommandAdapter<VkCallback>
    {
        void UseApi(IVkApi api);
    }
}
