namespace ScheduleVkManager.ChatBot.Commands.Adapters
{
    public interface ICommandAdapter<T>
    {
        CommandResult Execute(string command, T input);
    }
}
