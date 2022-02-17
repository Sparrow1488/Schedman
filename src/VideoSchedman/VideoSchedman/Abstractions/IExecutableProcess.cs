namespace VideoSchedman.Abstractions
{
    internal interface IExecutableProcess
    {
        Task StartAsync(IScriptBuilder builder);
    }
}
