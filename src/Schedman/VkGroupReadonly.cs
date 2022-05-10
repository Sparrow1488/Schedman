using VkNet.Abstractions;

namespace Schedman
{
    public class VkGroupReadonly
    {
        public VkGroupReadonly(IVkApi api, long groupId, string groupTitle = "")
        {
            _api = api;
            Id = groupId;
            Title = groupTitle;
        }

        private readonly IVkApi _api;

        public long Id { get; }
        public string Title { get; }
    }
}
