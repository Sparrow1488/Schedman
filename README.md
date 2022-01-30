# VkSchedman

## Описание

VkScheduleManager - утилита для администраторов и редакторов сообществ ВКонтакте, призванная сократить время на публикацию новых записей на стену паблика.

## Основной функционал

### Авторизация с помощью логина и пароля

``` C#
// в конфиге указан путь до файла .txt, в первой строке записан Логин, во второй Пароль
var authDataPath = ConfigurationManager.AppSettings["auth"];
var authData = new AuthorizeData(authDataPath);
var vkManager = new VkManager();
var authResult = await vkManager.AuthorizeAsync(authData);
if (authResult is false) {
    PrintErrors(vkManager.Errors);
    vkManager.ClearErrors();
    throw new Exception("Auth error");
}
else _logger.Success("Authorize success");
```

### Поиск группы для редактирования

``` C#
_group = await vkManager.GetGroupManagerAsync("group_name");
if (_group.Id == 0) {
    PrintErrors(vkManager.Errors);
    throw new Exception("Cannot find group");
}
else _logger.Success("Success found group id_" + _group.Id);
```

### Публикация записей по расписанию

``` C#
var posts = _postEditor.CreatePostRange();
_scheduler.Create(_times, 30, posts.Count());
posts = posts.Shuffle();
posts = _postEditor.SetSchedule(posts, _scheduler);
foreach (var post in posts) {
    try {
        var createdPost = await _group.AddPostAsync(post);
        _logger.Success("Post was success loaded");
    } catch (PostLimitException e) {
        _logger.Error(e.Message);
    }
}
```



## VkManager

### Описание

Основной класс, инкапсулирующий работу с библиотекой VkNet. Призван упростить взаимодействие с Api Vk.

### Важные методы

``` C#
// Открывает весь функционал класса
public async Task<bool> AuthorizeAsync(AuthorizeData authorizeData) 
```

``` C#
// Получает менеджера группы по введенному имени
public async Task<GroupManager> GetGroupManagerAsync(string groupName)
```

``` C#
// Получает коллекцию ошибок, возникших в ходе использования методов (будет пересмотрено)
public IList<string> GetErrors()
```

``` C#
// Очищает список ошибок
public void ClearErrors()
```



## GroupManager

### Описание

Класс для редактирования и изменения сообщества.

### Важные методы

``` C#
// Добавляет запись на стену сообщества
public async Task<CreatePost> AddPostAsync(CreatePost post)
```



## Scheduler

### Описание

Класс упрощает работу с датами и расписанием. На вход поступает коллекция из структур `TimeSpan`, иными словами расписание публикации записей.

### Важные методы

``` C#
// Создает коллекцию из DateTime в указанное в times время на каждый день
public void Create(IEnumerable<DateTime> times)
```

Далее идут аналогичные методы, но с более гибкой настройкой расписания.

``` C#
public void Create(DateTime firstTime, TimeSpan interval, int count)
```

``` C#
public void Create(IEnumerable<TimeSpan> times, int days, DateTime? startTime = null)
```

``` C#
 public void Create(IEnumerable<TimeSpan> times, int days, int limitCount, DateTime? startTime = null)
```



## PostEditor

### Описание

Класс упрощает и автоматизирует процесс создания и редактирования публикаций для дальнейшей публикации

### Важные методы

``` C#
// Создает коллекцию настроенных публикаций, согласно правилам зарузки интерфейса IAlbumsHandler
// cpecialPath использовать не предпочтительно. Рекоммендуется указывать путь к главной директории в файле App.config в поле "mainAlbumPath"
public IEnumerable<CreatePost> CreatePostRange(string cpecialPath = null)
```

``` C#
// Устанавливает для созданных записей расписание для публикации
public IEnumerable<CreatePost> SetSchedule(IEnumerable<CreatePost> posts, Scheduler scheduler)
```



## IAlbumsHandler

### Описание

Интерфейс для описания правил автоматического считывания файлов из локальных директорий. Используется как внутренний компонент утилиты (ScheduleVkManager) и впринцепе может не подвеграться изменениям.

### Важные методы

``` C#
// Устанавливает паттерн поиска вложенных (и не только) директорий
public bool SetPattern(string key, string pattern);
```

``` C#
public IDictionary<string, string> GetPatterns();
```

``` C#
// Получает альбом из локальной директории
public Album GetAlbum(string albumPath);
```
