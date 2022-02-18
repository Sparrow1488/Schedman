namespace VideoSchedman.Abstractions
{
    internal interface IExecutableProcess
    {
        Task StartAsync(string command);
    }
}
