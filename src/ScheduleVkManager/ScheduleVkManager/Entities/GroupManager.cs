using ScheduleVkManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace ScheduleVkManager.Entities
{
    public class GroupManager : IStorableErrors
    {
        public long Id { get; }
        private readonly VkApi _api;
        public IList<string> Errors { get; set; }

        public GroupManager(VkApi api, long groupId)
        {
            Id = groupId;
            _api = api;
        }

        public async Task<CreatePost> AddPostAsync(CreatePost post)
        {
            var uploadedPhotos = await UploadWallPhotosAsync(post.PhotosUrl);
            var postId = await _api.Wall.PostAsync(new WallPostParams()
            {
                Signed = false,
                OwnerId = -Id,
                Message = post.Message,
                FromGroup = true,
                Attachments = uploadedPhotos
            });
            post.Id = postId;
            return post;
        }

        private async Task<IEnumerable<Photo>> UploadWallPhotosAsync(IEnumerable<string> localUrls)
        {
            var result = new List<Photo>();
            foreach (var url in localUrls)
            {
                var uploadServer = await _api.Photo.GetWallUploadServerAsync(Id);
                var wc = new WebClient();
                var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, url));
                var photos = _api.Photo.SaveWallPhoto(responseFile, (ulong)_api.UserId, (ulong)Id, "Че происходит");
                result.AddRange(photos.ToArray());
            }
            return result;
        }

        public IList<string> GetErrors() => Errors;
        public void ClearErrors() => Errors = new List<string>();
    }
}
