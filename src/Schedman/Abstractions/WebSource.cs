using Schedman.Enums;

namespace Schedman.Abstractions
{
    public abstract class WebSource
    {
        public WebSource(string url) =>
            Url = url;

        public string Url { get; }
        public abstract WebSourceType Type { get; }
    }
}
