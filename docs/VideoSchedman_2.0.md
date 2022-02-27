## VideoSchedman 2.0

### Что нового

Итак, оглядываясь на свой предыдущий опыт в проектировании библиотек и программ, я могу сделать вывод, что говнокодить у меня получается лучше, чем кропотливо сидеть и обдумывать модульность и расширяемость программ. При проектировании **VideoSchedman** я не имел точного понимания как в принципе устроена библиотека FFMpeg и, не успев до конца разобраться, лез писать кодус. После конкретного code review, я пересмотрел свои взгляды на проектирование программ, а также больше познакомился с упомянутой ранее библиотекой, что внесло определенный вклад в чистоту и прозрачность моего решения. В первую очередь, я стремлюсь предоставить пользователям и разработчикам наиболее удобное взаимодействие с моим программным интерфейсом, чтобы с ходу можно было понять (минимально обращаясь к документации) как устроен тот или иной метод/класс. Поэтому, прикинув первичные сущности, я постарался продумать способ взаимодействия разработчиков с ними, параллельно продумывая будущую реализацию.

###  Пример

```C#
var videos = GetVideos(path);
var tempNames = new string[] { "nature", "animal", "city" };
var watermarkPath = GetWatermarkImagePath();

var ffmpeg = Factory.CreateFFMpeg();
var pipe = ffmpeg.CreatePipeline();
pipe.AddVideos(videos)
    .Subsequence(Subsec.TemplateName, tempNames)
    .Loop(Loop.Template, file => file.Duration < TimeSpan.From("00:00:10"), loopCount: 2)
    .Watermark(Watermark.StaticImage, path:watermarkPath, opacity:20)
    .Result.Quality(Quality.FHD);

var middlewareBuilder = new FileMiddlewareBuilder().ToSameTypes()
						   .ToTransportStream()
						   .Concat();
await ffmpeg.StartMiddlewareAsync(middlewareBuilder.Build(), pipe);
```

### Как все устроено

**Factory** - статический класс, отвечает за логику создания и построения объекта-типа **FFMpegEngine**. Основной метод `CreateFFMpeg()`.

**FFMpegEngine** - класс, инкапсулирует логику создания основной видеодорожки (**Pipeline**), а также ответственный за кодирование и конвертацию видеофайлов в итоговый проект. Содержит методы `CreatePipeline()`, `StartMiddlewareAsync()`.

**Pipeline** - класс, предоставляет "открытый интерфейс" настройки результирующего видеопроекта. Содержит методы `AddVideos()`, `Subsequence()`, `Loop()`, `Watermark()` и публичное поле Result, являющееся объектом-типа **FileMeta**. 

**FileMeta** - класс, позволяет настроить кодировку, разрешение, качество итогового файла.

**FileMiddleware** - класс, настраивается при помощи объекта-типа **FileMiddlewareBuilder**. Сам класс "запечатывает" в себе параметры в конвейер, необходимые для дальнейшей обработки файлов в классе **FFMpegEngine**.

**FileMiddlewareBuilder** - класс-строитель, позволяет создавать и настраивать **FileMiddleware**.







