using Serilog;
using VideoSchedman;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string ffmpeg = @"D:\games\ffmpeg\ffmpeg.exe";
string rootVideos = @"C:\Users\aleks\Downloads\test-videos-2\";
string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor(ffmpeg).Configure(config =>
{
    config.AddSrc(rootVideos + "1.mp4")
          .AddSrc(rootVideos + "2.mp4")
          .SaveTo("./", "result");
    resultPath = config.OutputFile.ToString();
});

Log.Information("Обрабатываем...");
//await editor.ConcatSourcesAsync();
Log.Information("Готово: " + File.Exists(resultPath));
