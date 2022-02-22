using VideoSchedman.Entities;
using VideoSchedman.Enums;

namespace VideoSchedman.Abstractions
{
    public interface IVideoEditor
    {
        IVideoEditor Configure(Action<Configuration> config);
        Task CacheAsTsFormatAsync();
        void CleanTsCache();
        Task ConcatSourcesAsync(ConcatType concatType);
    }
}
