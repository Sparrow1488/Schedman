using Microsoft.Extensions.DependencyInjection;
using Schedman.Abstractions;
using Schedman.Clients;
using Schedman.Commands;
using Schedman.Commands.Parameters;
using Schedman.Entities;
using Schedman.Exceptions;
using Schedman.Helpers;
using Schedman.Tools.IO;
using System;
using System.Collections.Generic;
using System.Linq;
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
            _client = new VkClient();
            _clientWrapper = new HttpClientWrapper();
        }

        public bool IsAuthorizated => _api.IsAuthorized;
        private readonly VkApi _api;
        private readonly IVkClient _client;
        private readonly HttpClientWrapper _clientWrapper;

        public async Task AuthorizateAsync(AccessPermission accessPermission)
        {
            try {
                await _client.SendRetryAsync(new VkAuthorizationCommand(_api, accessPermission));
            }
            catch (VkAuthException) {
                throw new SchedmanAuthorizationException();
            }
            catch (Exception ex) {
                throw new SchedmanException(ex.Message);
            }
        }

        public async Task<IEnumerable<Video>> GetVideosFromOwnAlbumAsync(string albumTitle)
        {
            var albums = await _client.SendRetryAsync<VkCollection<VideoAlbum>>(
                            new VkGetVideoAlbumsCommand(_api, VkVariables.MaxVideoAlbumsCountToGet));
            var foundAlbum = GetVideoAlbumOrThrow(albums, albumTitle);
            
            var param = new GetVideoParam(videoAlbumId: (long)foundAlbum.Id);
            var albumVideos = await _client.SendRetryAsync<VkCollection<Video>>(new VkGetVideoCommand(_api, param));
            return albumVideos.Cast<Video>();
        }

        private VideoAlbum GetVideoAlbumOrThrow(VkCollection<VideoAlbum> collection, string title) =>
            collection.Where(album => album.Title.ToUpper() == title.ToUpper()).FirstOrDefault()
                ?? throw new AlbumNotFoundException("Album not found");

        public async Task<byte[]> DownloadVideoAsync(Video video, IProgress<IntermediateProgressResult> downloadProgress = null)
        {
            var videoData = Array.Empty<byte>();
            Uri downloadUri = VkVideoHelper.SelectHighVideoQualitySource(video);
            
            if (downloadUri != null)
            {
                videoData = await _clientWrapper.DownloadDataAsync(downloadUri, downloadProgress);
            }

            return videoData;
        }

        public async Task<VkGroupManager> GetGroupManagerAsync(string groupTitle)
        {
            var foundGroup = await GetGroupOrDefaultByTitleAsync(groupTitle)
                                ?? throw new SchedmanGroupNotFoundException($"Cannot found group by name '{groupTitle}'");
            if (!foundGroup.IsAdmin)
                throw new SchedmanNoGroupAccess("You can't mange this group (Admin access denied)");

            return new VkGroupManager(_api, foundGroup?.Id ?? 0, foundGroup.Name);
        }

        public async Task<VkGroupReadonly> GetGroupAsync(string groupTitle)
        {
            var foundGroup = await GetGroupOrDefaultByTitleAsync(groupTitle)
                                ?? throw new SchedmanGroupNotFoundException($"Cannot found group by name '{groupTitle}'");

            return new VkGroupReadonly(_api, foundGroup?.Id ?? 0, foundGroup.Name);
        }

        private async Task<Group> GetGroupOrDefaultByTitleAsync(string groupTitle)
        {
            var userGroups = await _api.Groups.GetAsync(new GroupsGetParams() { UserId = _api.UserId });
            var groupsIdsList = userGroups.Where(gr => gr.Id != 0)
                                          .Select(gr => gr.Id.ToString())
                                          .ToList();
            var groups = await _api.Groups.GetByIdAsync(groupsIdsList, string.Empty, new GroupsFields());
            var foundGroup = groups?.FirstOrDefault(group => group.Name.ToLower().Contains(groupTitle.ToLower()));
            return foundGroup;
        }
    }
}
