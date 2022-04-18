# Schedman

С помощью готовых решений в библиотеке Schedman, Вы сможете автоматизировать некоторые рутинные процессы, преимущественно, администрирование группы в социальной сети ВКонтакте. На данный момент библиотека помогает автоматизировать такие процессы как:

- Загрузка публикаций на стену ВК по указанному расписанию
- Загрузка видеозаписей из публичных и приватных альбомов

Над чем предстоит работать:

- Рефакторинг кода
- Работа над чистой архитектурой
- Возможно, добавление некоторых забавных фич
- Стабильный релиз

## Примеры

**Авторизоваться**

```C#
var access = new AccessPermission(ConfigurationManager.AppSettings["accessFile"]);
var manager = new VkManager();
await manager.AuthorizateAsync(access);
```
**Получить все публикации**

```C#
var group = await manager.GetGroupManagerAsync("your_group_name");
var publishes = await group.GetPublishesAsync(page: 1, count: 20);
```
**Добавить публикацию в группу**
```C#
var imageSource = await group.UploadServer.UploadImageAsync(ConfigurationManager.AppSettings["imageFile"]);
var publishEntity = new VkPublishEntity() {
    Message = "Hello world!"
};
publishEntity.MediaCollection.Add(imageSource);
var result = await group.PublishAsync(publishEntity);
result.ThrowIfHasFails();
```
**Загрузить видео из альбома и сохранить их локально, используя утилиту**

```C#
var videos = await manager.GetVideosFromOwnAlbumAsync("my_album_name");
var saveConfig = new SaveServiceConfiguration("my_album_name - downloads", "./downloads");
var saveService = new SaveService(saveConfig);
foreach (var video in videos)
{
    var videoBytes = await manager.DownloadVideoAsync(video);
    await saveService.SaveLocalAsync(videoBytes, SaveFileInfo.Name(video.Title).Mp4());
    Console.WriteLine("SAVE => " + video.Title);
}
```

## Подробности

Вы можете ознакомиться с примерами к данной библиотеке, перейдя по следующим ссылкам:

* [Schedman.CI](https://github.com/Sparrow1488/Schedman/tree/master/src/Schedman.CI)
* [Schedman with Dependency Injection](https://github.com/Sparrow1488/Schedman/tree/master/src/Schedman.Examples/WithDependencyInjection)

