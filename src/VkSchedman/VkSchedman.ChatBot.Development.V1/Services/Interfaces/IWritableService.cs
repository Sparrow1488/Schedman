using VkSchedman.ChatBot.Commands;

namespace VkSchedman.ChatBot.Services.Interfaces
{
    public interface IWritableService
    {
        void Write(CommandResult input);
    }
}
