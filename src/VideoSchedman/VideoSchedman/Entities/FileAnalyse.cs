using Newtonsoft.Json;

namespace VideoSchedman.Entities
{
    public class FileAnalyse
    {
        public IEnumerable<FileAnalyseStream> Streams { get; set; }
    }

    public class FileAnalyseStream
    {
        public int Index { get; set; }
        [JsonProperty("codec_name")]
        public string CodecName { get; set; }
        [JsonProperty("codec_type")]
        public string CodecType { get; set; }
    }
}
