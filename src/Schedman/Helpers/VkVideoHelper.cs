using System;
using VkNet.Model.Attachments;

namespace Schedman.Helpers
{
    internal static class VkVideoHelper
    {
        public static Uri SelectHighVideoQualitySource(Video vkVideo)
        {
            Uri downloadUri = default;
            if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_1080?.ToString()))
                downloadUri = vkVideo.Files.Mp4_2160;
            else if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_1080?.ToString()))
                downloadUri = vkVideo.Files.Mp4_1440;
            else if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_1080?.ToString()))
                downloadUri = vkVideo.Files.Mp4_1080;
            else if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_720?.ToString()))
                downloadUri = vkVideo.Files.Mp4_720;
            else if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_480?.ToString()))
                downloadUri = vkVideo.Files.Mp4_480;
            else if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_360?.ToString()))
                downloadUri = vkVideo.Files.Mp4_360;
            else if (!string.IsNullOrWhiteSpace(vkVideo.Files.Mp4_240?.ToString()))
                downloadUri = vkVideo.Files.Mp4_240;
            return downloadUri ?? default;
        }
    }
}
