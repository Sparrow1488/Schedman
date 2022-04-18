using Schedman.Abstractions;
using Schedman.Entities;
using Schedman.Enums;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VkNet.Abstractions;
using VkNet.Model;

namespace Schedman.Data
{
    public class VkUploadServer : IUploadServer
    {
        internal VkUploadServer(IVkApi api, long groupId)
        {
            _api = api;
            _groupId = groupId;
        }

        private readonly long _groupId;
        private readonly IVkApi _api;

        public async Task<WebImage> UploadImageAsync(string filePath)
        {
            string responseFile = string.Empty;
            var uploadServer = await GetServerAsync(WebSourceType.Image);
            using (var client = new WebClient())
            {
                responseFile = Encoding.ASCII.GetString(client.UploadFile(uploadServer.UploadUrl, filePath));
            }
            return new WebImage(responseFile);
        }

        private async Task<UploadServerInfo> GetServerAsync(WebSourceType sourceType)
        {
            UploadServerInfo server = default;
            if (sourceType.Equals(WebSourceType.Image))
                server = await _api.Photo.GetWallUploadServerAsync(_groupId);
            else if (sourceType.Equals(WebSourceType.Video))
                throw new NotImplementedException();
            return server;
        }
    }
}
