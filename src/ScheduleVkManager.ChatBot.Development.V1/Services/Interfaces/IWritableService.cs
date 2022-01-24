using ScheduleVkManager.ChatBot.Commands;

namespace ScheduleVkManager.ChatBot.Services.Interfaces
{
    public interface IWritableService
    {
        void Write(CommandResult input);
    }
}
