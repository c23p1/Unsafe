# AwesomeFiles
### Тестовое задание SafeBoard Kaspersky 2025 от Группы анализа инфраструктуры

## Установка

### API
Для сборки проекта API необходимо:
- Зайти в директорию проекта (где находится compose.yaml)
- Убедиться в том, что директории AwesomeFiles и AwesomeLogs лежат в этой же папке (если нет - их необходимо создать)
- Запустить команду ```docker-compose up -d```
- Сервис запустится по адресу http://localhost
- Для добавления "потрясающих" файлов, необходимо загрузить их в директорию AwesomeFiles


### CLI
Для сборки проекта CLI необходимо:
- Зайти в директорию проекта
- Запустить команду ```dotnet publish src/CLI/```
- Запустить команду ```./src/CLI/bin/Release/net8.0/afcli```, далее использовать её как приложение afcli

Пример
```
./src/CLI/bin/Release/net8.0/afcli list -u http://localhost
```

## Использование

### Эндпоинты API
```[GET] /files``` - Получить список "потрясающих" файлов

```[POST] /archivation/initialize``` - Инициализировать процесс создания архива

```[GET] /archivation/getStatus/{processId}``` - Получить статус процесса создания архива

```[GET] /archivation/download/{processId}``` - Сохранить созданный архив

### Команды CLI
#### afcli - главная с ASCII изображением
```
     _                                         _____ _ _
    / \__      _____  ___  ___  _ __ ___   ___|  ___(_) | ___  ___
   / _ \ \ /\ / / _ \/ __|/ _ \| '_ ` _ \ / _ \ |_  | | |/ _ \/ __|
  / ___ \ V  V /  __/\__ \ (_) | | | | | |  __/  _| | | |  __/\__ \
 /_/   \_\_/\_/ \___||___/\___/|_| |_| |_|\___|_|   |_|_|\___||___/
  __________________
 | By Kaspersky Lab |
  ------------------
Description:
  CLI для управления "потрясающими" файлами

Usage:
  afcli [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  --version       Show version information

Commands:
  list         Получить список "потрясающих" файлов
  archivation  Управление задачами архивации файлов
```
#### afcli list [options] - Получить список "потрясающих" файлов
```
afcli list -u http://localhost
```

#### afcli archivation init \<fileNames\>... [options] - Инициализировать процесс создания архива
```
afcli archivation init file1 file2 fileN -u https://localhost:7151
```

#### afcli archivation status \<processId\> [options] - Получить статус процесса создания архива
```
afcli archivation status f69fb259-315c-434c-902e-ad9b29abfca2 -u https://localhost:7151
```

#### afcli archivation download \<processId\> [options] - Сохранить созданный архив
```
afcli archivation download f69fb259-315c-434c-902e-ad9b29abfca2 -d ~/Documents -u https://localhost:7151
```

#### Доступные флаги
```
Options:
  -u, --url <url>                  Адрес сервера с "потрясающими" файлами [default: http://localhost]
  -?, -h, --help                   Просмотр справки

  -d, --destination <destination>  Путь, по которому будет сохранён архив [default: .]
  ^^^^^
  Только для afcli download
```


## Реализованные задачи со звёздочкой
- Добавить персистентное хранилище для логов (файл)
- Написать dockerfile +compose.yaml для разворачивания сервиса
- Добавить кэширование архивов (кэшируются архивы завершённых процессов)
- Поддержать POSIX стандарт для CLI

## Особенности реализации и комментарии
- Одинаковые имена файлов при инициализации процесса не являются ошибкой, но воспринимаются программой как одно уникальное имя
- В проект API добавлен Swagger для удобства при тестировании
- При формировании архивов используется максимально возможный уровень сжатия для сохранения памяти
- Serilog⋅был помещён в Application, где в дальнейшем может быть использован для логгирования в сервисах

\
\
Шаталов Денис, C23-RTU@yandex.ru
