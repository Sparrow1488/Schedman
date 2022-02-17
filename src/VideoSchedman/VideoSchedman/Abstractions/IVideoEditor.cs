using VideoSchedman.Entities;

namespace VideoSchedman.Abstractions
{
    public interface IVideoEditor
    {
        IVideoEditor Configure(Action<Configuration> config);
        Task ConcatSourcesAsync();
    }
}
