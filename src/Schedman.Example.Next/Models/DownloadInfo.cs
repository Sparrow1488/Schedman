using System;
using System.Collections.Generic;
using System.Text;

namespace Schedman.Example.Next.Models
{
    public struct DownloadInfo
    {
        public string Source { get; set; }
        public string DownloaderVersion { get; set; }
        public List<DownloadFile> Files { get; set; }
        public DateTime DownloadedAt { get; set; }
        public string RepositoryLink { get; set; }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"[Source] {Source}\n");
            builder.AppendLine($"[Downloader][Version] {DownloaderVersion}\n");
            for (var i = 0; i < Files.Count; i++)
            {
                var file = Files[i];
                builder.AppendLine($"({i + 1}/{Files.Count}) Source: {file.SourceName}");
                builder.AppendLine($"Success: {file.IsSuccess}, Name: {file.Name} \n");
            }

            builder.AppendLine("Downloaded at " + DownloadedAt.ToString("f") + "\n");
            if (!string.IsNullOrWhiteSpace(RepositoryLink))
            {
                builder.AppendLine("GitHub repository link: " + RepositoryLink);
            }

            return builder.ToString();
        }
    }
}