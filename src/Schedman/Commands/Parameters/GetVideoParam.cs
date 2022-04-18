using Schedman.Helpers;

namespace Schedman.Commands.Parameters
{
    internal class GetVideoParam
    {
        public GetVideoParam(
            long videoAlbumId = -1, 
            long offset = 0, 
            int count = VkVariables.MaxVideosCountToGet)
        {
            VideoAlbumId = videoAlbumId;
            Offset = offset;
            Count = count;
        }

        public long VideoAlbumId { get; set; }
        public long Offset { get; set; }
        /// <summary>
        /// Max count is 200
        /// </summary>
        public int Count { get; set; }
    }
}
