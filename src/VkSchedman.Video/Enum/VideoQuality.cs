namespace VkSchedman.Video.Enum
{
    public class VideoQuality
    {
        private VideoQuality(string quality)
        {
            Quality = quality;
        }

        public readonly string Quality;

        public static readonly VideoQuality Preview = new VideoQuality("480x360");
        public static readonly VideoQuality HD = new VideoQuality("720x480");
        public static readonly VideoQuality FHD =  new VideoQuality("1920x1080");
    }
}
