using VkSchedman.Exceptions;
using VkSchedman.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using Serilog;
using Newtonsoft.Json;

namespace VkSchedman.Entities
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
            var postId = await TryAddPostAsync(post);
            post.Id = postId;
            return post;
        }

        public IList<string> GetErrors() => Errors;
        public void ClearErrors() => Errors = new List<string>();

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

        private async Task<long> TryAddPostAsync(CreatePost post)
        {
            long postId = 0;
            var uploadedPhotos = await UploadWallPhotosAsync(post.PhotosUrl);
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
                catch(JsonException)
                {
                    Log.Error($"Не удалось опубликовать запись. Повторная попытка ({attemptCurrent + 1}/{attemptMax})");
                }
                finally
                {
                    if(postId != 0)
                        attemptCurrent = 100000;
                    else
                        attemptCurrent++;
                }
            }
            if (postId == 0) throw new Exception("Не удалось опубликовать запись");
            return postId;
        }
    }
}