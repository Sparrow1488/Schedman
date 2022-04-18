using Schedman.Abstractions;
using Schedman.Commands.Parameters;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace Schedman.Commands
{
    internal class VkGetVideoCommand : VkCommandBase
    {
        public VkGetVideoCommand(IVkApi api, GetVideoParam param) : base(api)
        {
            _param = param;
        }

        private readonly GetVideoParam _param;
        private VkCollection<Video> _result;

        public override async Task ExecuteAsync()
        {
            _result = await Api.Video.GetAsync(new VideoGetParams()
            {
                AlbumId = _param.VideoAlbumId,
                Offset = _param.Offset,
                Count = _param.Count
            });
        }

        public override TResult GetResultOrDefault<TResult>() => _result as TResult;
    }
}
