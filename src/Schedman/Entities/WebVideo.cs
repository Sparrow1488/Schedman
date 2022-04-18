using Schedman.Abstractions;
using Schedman.Enums;

namespace Schedman.Entities
{
    public class WebVideo : WebSource
    {
        public WebVideo(string url) : base(url) { }

        public override WebSourceType Type => WebSourceType.Video;
    }
}
