using VideoSchedman;

var editor = new FFMpegEditor().Configure(config =>
{
    config.AddSrc("./video.mp4")
          .AddSrc("./video2.mp4")
          .SaveTo("./save-files/", "result");
});
await editor.ConcatSourcesAsync();
