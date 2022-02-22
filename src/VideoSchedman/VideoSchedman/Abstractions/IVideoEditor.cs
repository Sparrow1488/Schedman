using VideoSchedman.Entities;
using VideoSchedman.Enums;

namespace VideoSchedman.Abstractions
{
    public interface IVideoEditor
    {
        event LogAction OnCachedSource;
        event LogAction OnConvertedSource;
        delegate void LogAction(string message);

        IVideoEditor Configure(Action<Configuration> config);
        Task ConcatSourcesAsync(ConcatType concatType);
    }
}
