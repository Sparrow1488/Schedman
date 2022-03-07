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

Project.UseExistsProject("project_bdb68489-378e-4f1f-b514-ce96aa38bb11");
//Project.CreateProject();
var editor = new FFMpegEditor().Configure(config =>
                                config.RestoreSrc()
                                      .AddDistinctSrcRange(files)
                                      .SaveTo("Compilation")
                                      .Loop(file => file.Analyse.GetVideo().Duration <= 10, 2)
                                      .Quality(VideoQuality.FHD));

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingConcatConvertedViaTransportStream);
Log.Information("Успешно");