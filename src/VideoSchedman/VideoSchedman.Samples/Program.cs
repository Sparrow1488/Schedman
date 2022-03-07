using Serilog;
using VideoSchedman;
using VideoSchedman.Entities;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string rootVideos = @"D:\Закачки\ffmpeg_build\ffmpeg\bin\to_test\wait\asd";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

//Project.UseExistsProject("project_4752684e-fee7-4d71-a0f6-1d4d3e47ac95");
Project.CreateProject();
var editor = new FFMpegEditor().Configure(config =>
                                config.RestoreSrc()
                                      .AddDistinctSrcRange(files)
                                      .SaveTo("Compilation")
                                      .Quality(VideoQuality.FHD));

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingConcatConvertedViaTransportStream);
Log.Information("Успешно");