# Centuriin.CardGame.Core

## Русский 🇷🇺

Модульный движок для карточных игр на .NET 10 с ECS, обработкой команд и событийной моделью состояния. Проект находится на стадии прототипа; воспроизведение событий и восстановление партий ещё дорабатываются.

### Структура решения

| Проект | Назначение |
| --- | --- |
| `Engine` | Сущности, компоненты, игровые системы, команды, события и состояние партии. Бывший проект `Common`. |
| `Application` | Сессии, запуск партий, инициализация и события приложения. |
| `Extensions.DependencyInjection` | Регистрация движка и создание его зависимостей через Microsoft.Extensions.DependencyInjection. |
| `Infrastructure/Storage/Abstractions` | Контракты репозиториев описаний партий, снимков, событий, колод и шаблонов. |
| `InMemory` | Шаблоны стандартных карт и демонстрационные шаблоны зон в памяти. |
| `Drunkard` | Разрабатываемый модуль игры «Пьяница». |
| `Console` | Пример подключения модулей и запуска сессий с демонстрационными репозиториями. |
| `Engine.Tests`, `Application.Tests` | Модульные тесты движка и слоя приложения. |

### Сборка и запуск

Требуются .NET SDK 10 или новее с поддержкой целевой платформы `net10.0` и среда выполнения .NET 10.

Из корня репозитория:

```shell
dotnet restore Centuriin.CardGame.Core.slnx
dotnet build Centuriin.CardGame.Core.slnx --no-restore
dotnet run --project Console/Console.csproj --no-build
```

Запуск тестов после сборки:

```shell
dotnet run --project Engine.Tests/Engine.Tests.csproj --no-build -- --progress off
dotnet run --project Application.Tests/Application.Tests.csproj --no-build -- --progress off
```

Тестовые проекты используют xUnit v3 и Microsoft.Testing.Platform.

### Текущее состояние

- Реализованы сущности игроков, карт и зон, компоненты, валидация команд, конфигурация систем и доставка событий.
- Ведущая игра (`MasterGame`) генерирует и применяет события; реплика (`ReplicaGame`) подготавливает команды для внешнего исполнения и применяет полученные пакеты событий.
- Применение записанных событий отделено от генерации последующих событий. Начальная раздача и порядок игроков пока используют случайное перемешивание без задаваемого seed.
- `Drunkard` пока использует `FakeCommand`; полный игровой цикл и определение победителя ещё не реализованы.
- `UseInMemoryStorage()` регистрирует репозиторий шаблонов зон. Репозитории колод, описаний партий и снимков в примере регистрируются отдельно в `Console/Program.cs`.
- Сохранение истории, восстановление и создание реплик ещё дорабатываются. Консольный пример демонстрирует подключение и запуск сессий.

XML-документация находится в исходниках C#. Реализации наследуют документацию контрактов через `<inheritdoc/>`; при сборке библиотек создаются XML-файлы документации.

### Лицензирование

Текущие библиотечные проекты объявляют лицензию **MIT** в метаданных пакетов. Тексты лицензии находятся в файлах `LICENSE.txt` в каталогах `Engine`, `Drunkard` и `Extensions.DependencyInjection`.

Для будущего отдельного ядра коллекционной карточной игры планируется **GPL-3.0-only**. Этот модуль пока не входит в решение.

---

<details>
<summary><b>English version</b></summary>

## English 🇬🇧

A modular card game engine on .NET 10 with ECS, command processing, and an event-based state model. The project is a prototype; event replay and game restoration are still being developed.

### Solution structure

| Project | Purpose |
| --- | --- |
| `Engine` | Entities, components, game systems, commands, events, and game state. Formerly named `Common`. |
| `Application` | Sessions, game startup, initialization, and application events. |
| `Extensions.DependencyInjection` | Engine registration and dependency creation through Microsoft.Extensions.DependencyInjection. |
| `Infrastructure/Storage/Abstractions` | Repository contracts for game descriptions, snapshots, events, decks, and templates. |
| `InMemory` | Standard card templates and demonstration zone templates held in memory. |
| `Drunkard` | The Drunkard game module under development. |
| `Console` | A module registration and session startup example using demonstration repositories. |
| `Engine.Tests`, `Application.Tests` | Unit tests for the engine and application layer. |

### Build and run

Requires .NET SDK 10 or later with support for the `net10.0` target framework, and the .NET 10 runtime.

From the repository root:

```shell
dotnet restore Centuriin.CardGame.Core.slnx
dotnet build Centuriin.CardGame.Core.slnx --no-restore
dotnet run --project Console/Console.csproj --no-build
```

Run the tests after building:

```shell
dotnet run --project Engine.Tests/Engine.Tests.csproj --no-build -- --progress off
dotnet run --project Application.Tests/Application.Tests.csproj --no-build -- --progress off
```

The test projects use xUnit v3 and Microsoft.Testing.Platform.

### Current status

- Player, card, and zone entities, components, command validation, system configuration, and event dispatch are implemented.
- The authoritative game (`MasterGame`) produces and applies events; a replica (`ReplicaGame`) prepares commands for external execution and applies received event batches.
- Applying recorded events is separate from producing successor events. Initial dealing and player order currently use random shuffling without a configurable seed.
- `Drunkard` currently uses `FakeCommand`; a complete game loop and winner determination are not implemented yet.
- `UseInMemoryStorage()` registers the zone template repository. The example registers deck, game description, and snapshot repositories separately in `Console/Program.cs`.
- History persistence, restoration, and replica creation are still being developed. The console example demonstrates registration and session startup.

XML documentation is included in the C# sources. Implementations inherit contract documentation through `<inheritdoc/>`; library builds produce XML documentation files.

### Licensing

The current library projects declare **MIT** in their package metadata. License texts are available in the `LICENSE.txt` files under `Engine`, `Drunkard`, and `Extensions.DependencyInjection`.

The planned separate collectible card game core is intended to use **GPL-3.0-only**. That module is not part of the solution yet.

</details>
