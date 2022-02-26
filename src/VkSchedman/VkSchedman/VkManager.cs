using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService.Exceptions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;
using VkSchedman.Entities;
using VkSchedman.Exceptions;
using VkSchedman.Interfaces;

namespace VkSchedman
{
    public class VkManager : IStorableErrors
    {
        public VkManager()
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            _api = new VkApi(services);
            Errors = new List<string>();
        }

        private readonly VkApi _api;
        public IList<string> Errors { get; private set; }

        public async Task<bool> AuthorizeAsync(AuthorizeData authorizeData)
        {
            bool authSuccess = false;
            var emptyContext = new ValidationContext(authorizeData);
            var validationErrors = authorizeData.Validate(emptyContext);

            if(validationErrors.Count() > 0)
                foreach (var error in validationErrors)
                    Errors.Add(error.ErrorMessage);
            else authSuccess = await TryAuthorizeAsync(_api, authorizeData);

            return authSuccess;
        }

        public async Task<VkCollection<Video>> GetVideosFromAlbumAsync(string albumTitle, int count = 100)
        {
            int maxVideosInRequest = 200;
            int downloadIterations = count / maxVideosInRequest;
            var albums = await _api.Video.GetAlbumsAsync(count: 100);
            var foundAlbum = albums.Where(album => album.Title.ToUpper() == albumTitle.ToUpper()).FirstOrDefault();
            if (foundAlbum is null)
                throw new AlbumNotFoundException("Album not found");
            
            var videos = new List<Video>();
            for (int i = 1; i <= downloadIterations + 1; i++)
            {
                videos.AddRange(await _api.Video.GetAsync(new VideoGetParams()
                {
                    AlbumId = foundAlbum.Id,
                    Offset = videos.Count,
                    Count = maxVideosInRequest
                }));
            }
            return new VkCollection<Video>((ulong)videos.Count, videos);
        }

        public async Task DownloadVideosAsync(VkCollection<Video> videos, string albumTitle = "")
        {
            for (int i = 0; i < videos.Count; i++)
            {
                var video = videos[i];
                var data = DownloadVideo(video);
                string videoTitle = video.Title.Replace("\\", "") // make valid path
                                               .Replace("/", "");
                string saveDirectory = $"./downloads" + (!string.IsNullOrWhiteSpace(albumTitle) ? $"/{albumTitle}" : "");
                Directory.CreateDirectory(saveDirectory);
                string saveVideoName = Path.Combine(saveDirectory, videoTitle);
                var existsFiles = Directory.GetFiles(saveDirectory)
                                           .Where(file => file.Contains(videoTitle)).Count();
                saveVideoName += existsFiles > 0 ? $"({existsFiles})" : "";
                if (data.Length > 0)
                    await File.WriteAllBytesAsync(saveVideoName + ".mp4", data);
                Log.Information($"[{i+1}/{videos.Count}] Downloaded \"{video.Title}\"");
            }
        }

        public async Task<GroupManager> GetGroupManagerAsync(string groupName)
        {
            var userGroups = await _api.Groups.GetAsync(new GroupsGetParams() {
                UserId = _api.UserId,
            });
            var groupsId = new List<string>(userGroups.Where(gr => gr.Id != 0)
                                                      .Select(gr => gr.Id.ToString()));
            var groups = await _api.Groups.GetByIdAsync(groupsId, "", new GroupsFields());
            var foundGroup = groups?.Where(group => group.Name.ToLower().Contains(groupName.ToLower()))?.FirstOrDefault();
            
            if(foundGroup == null) {
                Errors.Add("Connot found group named " + groupName);
            }
            return new GroupManager(_api, foundGroup?.Id ?? 0);
        }

        public void ClearErrors() => Errors = new List<string>();
        public IList<string> GetErrors() => Errors;

        private async Task<bool> TryAuthorizeAsync(VkApi api, AuthorizeData authorizeData)
        {
            bool resultSuccess = true;
            try
            {
                await api.AuthorizeAsync(new ApiAuthParams
                {
                    Login = authorizeData.Login,
                    Password = authorizeData.Password,
                });
            }
            catch (VkAuthException authError)
            {
                Errors.Add(authError.Message);
            }
            catch
            {
                resultSuccess = false;
            }

            return resultSuccess;
        }

        private byte[] DownloadVideo(Video video)
        {
            var videoData = new byte[0];
            using (var client = new WebClient())
                videoData = client.DownloadData(video.Files.Mp4_1080);
            
            return videoData;
        }

    }
}
