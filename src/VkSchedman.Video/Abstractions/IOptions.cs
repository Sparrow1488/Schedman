namespace VkSchedman.Video.Abstractions
{
    public interface IOptions
    {
        string Build();
        void AddCommand(string command);
    }
}
