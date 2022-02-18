using VideoSchedman;

string ffmpeg = @"D:\games\ffmpeg\ffmpeg.exe";
string rootVideos = @"C:\Users\aleks\Downloads\test-videos-2\";
string resultPath = string.Empty;

Console.WriteLine("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    config.AddSrc(rootVideos + "1.mp4")
          .AddSrc(rootVideos + "2.mp4")
          .SaveTo("./", "result");
    resultPath = config.OutputFile.ToString();
});

Console.WriteLine("Обрабатываем...");
await editor.ConcatSourcesAsync();
Console.WriteLine("Готово: " + File.Exists(resultPath));
