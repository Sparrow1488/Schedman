using Serilog;
using VideoSchedman;
using VideoSchedman.Entities;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

//string rootVideos = @"C:\Users\aleks\Downloads\videos";
//string rootVideos = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\VkSchedman\src\VideoSchedman\VideoSchedman.Samples\TestFiles\test2\no";
string rootVideos = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\VkSchedman\src\VideoSchedman\VideoSchedman.Samples\TestFiles\test2\test";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

Project.UseExistsProject("project_07f6e8c4-4147-41c2-894d-0872d3164311");
//Project.CreateProject();
var editor = new FFMpegEditor().Configure(config =>
                                config.RestoreSrc()
                                      .SaveTo("Compilation")
                                      .Loop(file => file.Analyse.GetVideo().Duration <= 11, 2)
                                      .Quality(VideoQuality.FHD));

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingConcatConvertedViaTransportStream);
Log.Information("Успешно");