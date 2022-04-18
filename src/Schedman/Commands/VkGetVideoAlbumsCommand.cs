using Schedman.Abstractions;
using System;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace Schedman.Commands
{
    internal class VkGetVideoAlbumsCommand : VkCommandBase
    {
        public VkGetVideoAlbumsCommand(IVkApi api, int count) : base(api) 
        {
            _albumsCount = count;
        }

        private VkCollection<VideoAlbum> _result;
        private readonly int _albumsCount;

        public override async Task ExecuteAsync()
        {
            _result = await Api.Video.GetAlbumsAsync(count: _albumsCount);
        }

        public override TResult GetResultOrDefault<TResult>() => _result as TResult;
    }
}
