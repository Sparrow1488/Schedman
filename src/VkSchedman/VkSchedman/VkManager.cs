using Microsoft.Extensions.DependencyInjection;
using VkSchedman.Entities;
using VkSchedman.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VkNet;
using VkNet.AudioBypassService.Exceptions;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkSchedman.Exceptions;
using VkNet.Utils;
using VkNet.Model.Attachments;
using System.Net;
using System;
using Serilog;
using System.IO;
using AngleSharp.Html.Parser;

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

            if(validationErrors.Count() > 0) {
                foreach (var error in validationErrors) {
                    Errors.Add(error.ErrorMessage);
                }
            }
            else {
                authSuccess = await TryAuthorizeAsync(_api, authorizeData);
            }

            return authSuccess;
        }

        public async Task<VkCollection<Video>> GetVideosFromAlbumAsync(string albumTitle)
        {
            var albums = await _api.Video.GetAlbumsAsync(count: 100);
            var foundAlbum = albums.Where(album => album.Title.ToUpper() == albumTitle.ToUpper()).FirstOrDefault();
            if (foundAlbum is null)
                throw new AlbumNotFoundException("");
            var videos = await _api.Video.GetAsync(new VideoGetParams()
            {
                AlbumId = foundAlbum.Id
            });
            return videos;
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
            string htmlVkPlayer = "";
            using (var client = CreateWebClient())
                htmlVkPlayer = client.DownloadString($"http://m.vk.com/video{video.OwnerId}_{video.Id}");
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlVkPlayer);
            var vkPlayer = document.QuerySelector(".vv_inline_video");
            var videoSources = vkPlayer.QuerySelectorAll("source")
                                        .Where(elem => elem.GetAttribute("type") == "video/mp4")
                                        .ToArray();
            var bestQualitySource = videoSources[0].GetAttribute("src");

            byte[] videoData = new byte[0];
            using (var client = CreateWebClient())
                videoData = client.DownloadData(bestQualitySource);
            
            return videoData;
        }

        private WebClient CreateWebClient()
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36");
            //client.Headers.Add("Cookie", "remixnsid=999a3fb1077ffc518bc9c8b9fc1b053604ca10d3bbcf698d15c6484594dabe1dbd5a96f2c238793b471e8; remixsid=1_6CHzMrPKg2i1LoVDnrGfYyRvMX1h7-31xgac_UgzNSVQXYznXEzDQAK1NqjMNP7PSrw4lFPKnwC6PCrX3hFTSQ;");
            client.Headers.Add("Cookie", "remixQUIC=1; remixflash=0.0.0; remixscreen_width=1366; remixscreen_height=768; remixscreen_depth=24; remixdt=0; tmr_lvid=05f35707f7ec075e5841b98d2c6ebd8f; tmr_lvidTS=1638717699149; remixstid=e51c9faff268ba3f6c5d9099ad015d94e5aa9b695a8eaf58d243e35b2cc122e84bd0bb17ee40b7ff8abc8; remixuas=NGIxNGExZGUzZjkyMmI5MDk1NzZiYWY4; remixab=1; remixlang=100; remixrefkey=36ae53b1a349b277e2; remixseenads=0; remixscreen_orient=1; remixgp=95cde981785780b21e62adf1c4b16e8c; remixscreen_dpr=1.100000023841858; remixscreen_winzoom=1.10; remixnsid=999a3fb1077ffc518bc9c8b9fc1b053604ca10d3bbcf698d15c6484594dabe1dbd5a96f2c238793b471e8; remixsid=1_6CHzMrPKg2i1LoVDnrGfYyRvMX1h7-31xgac_UgzNSVQXYznXEzDQAK1NqjMNP7PSrw4lFPKnwC6PCrX3hFTSQ; remixua=41%7C-1%7C194%7C3261248227; remixaudio_show_alert_today=0; remixmdevice=1366/768/1/!!-!!!!!; remixdark_color_scheme=1; remixcolor_scheme_mode=auto; remixff=1011111111; remixcurr_audio=undefined; remixmvk-fp=474ef59f77db5e5fe758883e1724747e; tmr_detect=1%7C1645812811636; remixmdv=hBb5cYtgUVWIaudH; remixstickers_hash=b43161d3350153b4f74460fa5b7f846b; tmr_reqNum=1273");
            client.Headers.Add("Accept", "*/*");
            return client;
        }
    }
}
