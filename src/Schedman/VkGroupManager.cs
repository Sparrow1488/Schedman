using Schedman.Abstractions;
using Schedman.Data;
using Schedman.Entities;
using Schedman.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;

namespace Schedman
{
    public class VkGroupManager : GroupManager<VkPublishEntity>
    {
        public VkGroupManager(IVkApi api, long groupId, string groupTitle = "") 
        {
            _api = api;
            Id = groupId;
            Title = groupTitle;
        }

        private readonly IVkApi _api;

        public override long Id { get; internal set; }
        public override string Title { get; internal set; }
        public IUploadServer UploadServer => new VkUploadServer(_api, Id);

        public override async Task PublishAsync(VkPublishEntity post, Action<VkPublishEntity> onPublishFailed)
        {
            long postId = 0;
            var responses = post.MediaCollection.Images.Select(img => img.Url);
            var photosList = new List<Photo>();
            foreach (var response in responses)
            {
                var photos = _api.Photo.SaveWallPhoto(response, (ulong)_api.UserId, (ulong)Id, "Posted by : https://github.com/Sparrow1488/Schedman");
                photosList.AddRange(photos.ToArray());
            }

            int attemptMax = 3;
            int attemptCurrent = 0;
            while (attemptCurrent < attemptMax)
            {
                try
                {
                    postId = await _api.Wall.PostAsync(new WallPostParams()
                    {
                        Signed = false,
                        OwnerId = -Id,
                        Message = post.Message,
                        FromGroup = true,
                        Attachments = photosList,
                    });
                }
                catch (JsonException)
                {
                }
                finally
                {
                    if (postId != 0)
                        attemptCurrent = 100000;
                    else
                        attemptCurrent++;
                }
            }
            if (postId == 0) throw new SchedmanPublishFailedException();
            post.Uid = postId;
        }

        public override async Task<IEnumerable<VkPublishEntity>> GetPublishesAsync(int page = 0, int count = 20)
        {
            var publishesList = new List<VkPublishEntity>();
            var wallParams = new WallGetParams()
            {
                Count = (ulong)count,
                Offset = (ulong)page,
                OwnerId = -Id
            };
            var wallObjects = await _api.Wall.GetAsync(wallParams);
            var posts = wallObjects.WallPosts.Cast<Post>();
            foreach (var post in posts)
            {
                var publish = new VkPublishEntity();
                publish.Message = post.Text;
                publish.Uid = post?.Id ?? 0;
                var attachments = post.Attachments;
                foreach (var attach in attachments)
                {
                    string attachTypeUpper = attach.Type.Name.ToUpper();
                    if (attachTypeUpper == "VIDEO")
                    {
                        var webVideo = await GetWebVideoAsync(
                                        attach.Instance.Id ?? 0,
                                        attach.Instance.OwnerId ?? 0);
                        publish.MediaCollection.Add(webVideo);
                    }
                    if (attachTypeUpper == "PHOTO")
                    {
                        var webImage = await GetWebImageAsync(
                                        attach.Instance.Id ?? 0,
                                        attach.Instance.OwnerId ?? 0);
                        publish.MediaCollection.Add(webImage);
                    }
                }
                publishesList.Add(publish);
            }
            return publishesList;
        }

        private async Task<WebVideo> GetWebVideoAsync(long id, long ownerId)
        {
            string videoUrl = string.Empty;
            var videoParams = new VideoGetParams()
            {
                OwnerId = ownerId,
            };
            var videosCollection = await _api.Video.GetAsync(videoParams);
            var foundVideo = videosCollection?.FirstOrDefault(
                                video => video.Id == id);
            if (foundVideo != null)
            {
                videoUrl = foundVideo?.Files?.Mp4_480?.ToString();
            }
            return new WebVideo(videoUrl);
        }

        private async Task<WebImage> GetWebImageAsync(long id, long ownerId)
        {
            string photoUrl = string.Empty;
            var photosId = new string[]{ ownerId + "_" + id };
            var photosCollection = await _api.Photo.GetByIdAsync(photosId);
            var photo = photosCollection?.Cast<Photo>()?.FirstOrDefault();
            if (photo != null)
            {
                photoUrl = photo.Sizes.LastOrDefault()?.Url?.ToString();
            }
            return new WebImage(photoUrl);
        }
    }
}
