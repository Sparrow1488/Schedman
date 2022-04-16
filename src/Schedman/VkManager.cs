using Microsoft.Extensions.DependencyInjection;
using Schedman.Entities;
using Schedman.Exceptions;
using Schedman.Interfaces;
using System;
using System.Collections.Generic;
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

namespace Schedman
{
    public sealed class VkManager : IAuthorizableSchedman
    {
        public VkManager()
        {
            var services = new ServiceCollection();
            services.AddAudioBypass();
            _api = new VkApi(services);
        }


        public delegate void LoadProgress(int percent);
        public event LoadProgress OnLoadProgress;

        private readonly VkApi _api;
        public bool IsAuthorizated => _api.IsAuthorized;

        public async Task AuthorizateAsync(AccessPermission accessPermission)
        {
            var authSuccess = await TryAuthorizeAsync(_api, accessPermission);
            if (!authSuccess)
                throw new SchedmanAuthorizationException();
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

        public async Task DownloadVideosAsync(VkCollection<Video> videos, string saveAlbumTitle = "")
        {
            for (int i = 0; i < videos.Count; i++)
            {
                var video = videos[i];
                var data = await DownloadVideoAsync(video);
                await SaveVideoLocalAsync(video, data, saveAlbumTitle);
            }
        }

        public async Task<byte[]> DownloadVideoAsync(Video video)
        {
            var videoData = new byte[0];
            Uri downloadUri = null;
            if (!string.IsNullOrWhiteSpace(video.Files.Mp4_1080?.ToString()))
                downloadUri = video.Files.Mp4_1080;
            else if (!string.IsNullOrWhiteSpace(video.Files.Mp4_720?.ToString()))
                downloadUri = video.Files.Mp4_720;
            else if (!string.IsNullOrWhiteSpace(video.Files.Mp4_480?.ToString()))
                downloadUri = video.Files.Mp4_480;
            else if (!string.IsNullOrWhiteSpace(video.Files.Mp4_360?.ToString()))
                downloadUri = video.Files.Mp4_360;
            else if (!string.IsNullOrWhiteSpace(video.Files.Mp4_240?.ToString()))
                downloadUri = video.Files.Mp4_240;
            if (downloadUri != null)
            {
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += DownloadVideoProgress;
                    videoData = await client.DownloadDataTaskAsync(downloadUri);
                }
            }

            return videoData;
        }

        public async Task SaveVideoLocalAsync(Video video, byte[] data, string saveAlbumTitle = "")
        {
            string videoTitle = video.Title.Replace("\\", "") // make valid path
                                               .Replace("/", "");
            string saveDirectory = $"./downloads" + (!string.IsNullOrWhiteSpace(saveAlbumTitle) ? $"/{saveAlbumTitle}" : "");
            Directory.CreateDirectory(saveDirectory);
            string saveVideoName = Path.Combine(saveDirectory, videoTitle);
            var existsFiles = Directory.GetFiles(saveDirectory)
                                       .Where(file => file.Contains(videoTitle)).Count();
            saveVideoName += existsFiles > 0 ? $"({existsFiles})" : "";
            if (data.Length > 0)
                await File.WriteAllBytesAsync(saveVideoName + ".mp4", data);
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
            
            return new GroupManager(_api, foundGroup?.Id ?? 0, foundGroup.Name);
        }

        private async Task<bool> TryAuthorizeAsync(VkApi api, AccessPermission authorizeData)
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
            catch (VkAuthException)
            {
                throw new SchedmanAuthorizationException();
            }
            catch
            {
                resultSuccess = false;
            }

            return resultSuccess;
        }

        private void DownloadVideoProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            var onePercent = (e.TotalBytesToReceive / 100);
            int percent;
            unchecked
            {
                percent = (int)(e.BytesReceived / onePercent);
            };
            OnLoadProgress?.Invoke(percent);
        }

        Task IAuthorizableSchedman.AuthorizateAsync(AccessPermission accessPermission)
        {
            throw new NotImplementedException();
        }
    }
}
