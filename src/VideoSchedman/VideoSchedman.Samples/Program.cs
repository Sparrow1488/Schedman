﻿using Serilog;
using VideoSchedman;
using VideoSchedman.Enums;

Log.Logger = new LoggerConfiguration()
              .MinimumLevel
              .Debug()
              .WriteTo.Console()
              .CreateLogger();

string rootVideos = @"C:\Users\aleks\OneDrive\Desktop\Илья\Repositories\VkSchedman\src\VideoSchedman\VideoSchedman.Samples\bin\Debug\net6.0\cached-files\project_c6149ba7-3dcf-4e57-b85b-a682eebfc781";
var files = Directory.GetFiles(rootVideos).ToList();
string resultPath = string.Empty;

Log.Information("Запускаем");

var editor = new FFMpegEditor().Configure(config =>
                         config.AddSrcRange(files)
                               .SaveTo("concated-file-name")
                               .Quality(VideoQuality.FHD));
editor.OnCachedSource += (cached) => Log.Information(cached);
editor.OnConvertedSource += (converted) => Log.Information(converted);

Log.Information($"Добавлены файлы из папки \"{rootVideos}\" ({files.Count})");

await editor.ConcatSourcesAsync(ConcatType.ReencodingConcatConvertedViaTransportStream);
Log.Information("Успешно");