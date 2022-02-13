using System.Diagnostics;

namespace VkSchedman.Video
{
    public class VideoEditor
    {
        private const string _ffmpegPath = @"D:\games\ffmpeg\ffmpeg.exe";
        private const string _testVideoPath = @"D:\games\ffmpeg\video.mp4";

        public void TestConvertToAvi()
        {
            var ffmpegStartInfo = new ProcessStartInfo()
            {
                FileName = _ffmpegPath,
                Arguments = $"-y -i video.mp4 video.avi",
                WorkingDirectory = @"D:\games\ffmpeg\",
                CreateNoWindow = true,
                UseShellExecute = false
            };

            using (var process = new Process() { StartInfo = ffmpegStartInfo })
            {
                process.Start();
                process.WaitForExit();
            }
        }
    }
}