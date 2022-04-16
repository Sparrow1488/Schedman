using Schedman.Abstractions;
using Schedman.Entities;
using Schedman.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public override async Task PublishAsync(VkPublishEntity post)
        {
            var postId = await TryAddPostAsync(post);
            post.SetUid(postId);
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

        private async Task<long> TryAddPostAsync(VkPublishEntity post)
        {
            long postId = 0;
            var urls = post.MediaCollection.Images.Select(img => img.Url);
            var uploadedPhotos = await UploadWallPhotosAsync(urls);
            DateTime? schedule;

            if (post.Schedule < DateTime.Now)
                throw new InvalidInputDateException("You can't create post with past date!");
            else schedule = post.Schedule;

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
                        Attachments = uploadedPhotos,
                        PublishDate = schedule
                    });
                }
                catch (JsonException)
                {
                    //Logger.Error($"Не удалось опубликовать запись. Повторная попытка ({attemptCurrent + 1}/{attemptMax})");
                }
                finally
                {
                    if (postId != 0)
                        attemptCurrent = 100000;
                    else
                        attemptCurrent++;
                }
            }
            if (postId == 0) throw new Exception("Не удалось опубликовать запись");
            return postId;
        }

        public override async Task<IEnumerable<VkPublishEntity>> GetPublishesAsync()
        {
            var publishesList = new List<VkPublishEntity>();
            var wallParams = new WallGetParams()
            {
                Count = 5,
                OwnerId = -Id
            };
            var wallObjects = await _api.Wall.GetAsync(wallParams);
            var posts = wallObjects.WallPosts.Cast<Post>();
            foreach (var post in posts)
            {
                var publish = new VkPublishEntity();
                publish.SetMessage(post.Text);
                publish.SetUid(post.Id ?? -1);
                var attachments = post.Attachments;
                foreach (var attach in attachments)
                {
                    if(attach.Type.Name.ToUpper() == "VIDEO")
                    {
                        var webVideo = await GetWebVideoByIdAsync(
                                attach.Instance.Id ?? 0,
                                attach.Instance.OwnerId ?? 0);
                        publish.MediaCollection.Add(webVideo);

                    }
                }
                publishesList.Add(publish);
            }
            return publishesList;
        }

        private async Task<WebVideo> GetWebVideoByIdAsync(long id, long ownerId)
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
                videoUrl = foundVideo.Files.Mp4_480.ToString();
            }
            return new WebVideo(videoUrl);
        }
    }
}
