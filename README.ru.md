[English](README.md) | **Русский**

# HitMaster3D Test Task

Прототип шутера в духе hit-master. Игрок бежит между точками боя и стреляет по врагам тапом или кликом. Когда точка зачищена, игрок идёт к следующей. После последней точки уровень перезапускается.

![Gameplay](Docs/Gameplay.gif)

**Стек:** Unity 6 (6000.4.3f1), URP, Zenject (Extenject), UniTask, Addressables.

## Запуск

- **В редакторе:** открыть `Assets/Scenes/Initialize.unity` и нажать Play. Запуск со сцены `Main` не поддерживается: сервисы и контент загружаются в `Initialize`.
- **Билд:** сначала собрать контент Addressables (`Window → Asset Management → Addressables → Groups → Build → New Build → Default Build Script`), затем собрать плеер.
- **После добавления или переноса контента:** запустить `Tools/Addressables/Setup`, затем `Tools/Addressables/Analyze`. Setup выставляет группы, адреса и лейблы. Руками их не править.

## Слои

Весь код проекта лежит в `Assets/Core`. Каждый слой — отдельная сборка (asmdef), поэтому направление зависимостей проверяет компилятор.

```
Infrastructures  ←  Services  ←  App
       ↑               ↑
       └──── Logic ────┘
```

| Слой | Что внутри |
|---|---|
| `Infrastructures` | Базовые классы и абстракции: `Component`/`View`, стейт-машина, интерфейсы сервисов, `IGameFlow`, `IGameRestart`. Ни от кого не зависит. |
| `Services` | Реализации глобальных сервисов: ввод, страницы и UI-фабрика, загрузка сцен, апдейты, загрузка контента. |
| `App` | Корень приложения: `Bootstrap`, `GlobalInstaller`, `GameStateMachine` и её состояния. |
| `Logic` | Геймплей, по папке на фичу: игрок, враги, бой, пули, пул, спавнеры, камера, игровая фабрика, инсталлеры сцены. |

`App` и `Logic` друг о друге не знают. Приложение запускает сцену через `IGameFlow`, а сцена просит рестарт через `IGameRestart`.

## Поток запуска

```
Bootstrap → BootstrapState → LoadContentState → LoadLevelState → GameLoopState → IGameFlow
```

1. `Bootstrap` — точка входа на сцене `Initialize`. Запускает `GameStateMachine`.
2. `LoadContentState` один раз за сессию загружает лейблы `core` и `ui` и показывает при этом страницу загрузки. Если загрузка упала, показывает страницу ошибки с Retry.
3. `LoadLevelState` грузит сцену `Main` и достаёт `IGameFlow` из её `SceneContext`.
4. `GameLoopState` ждёт `StartGame()` и прокидывает во flow `Updating`/`FixedUpdating`.
5. `DemoGameFlowManager` выбирает уровень, загружает его лейбл `level_N` и собирает уровень: уровень → игрок → камера → точки боя → `BattleManager` → игровая страница.

## Ключевые решения

- **Рестарт через абстракцию.** `BattleManager` только поднимает `Completed`. Решение о рестарте принимает `DemoGameFlowManager` и вызывает `IGameRestart`. `GameRestartService` в `App` на следующем кадре освобождает `GameStateMachine` и заново входит в `BootstrapState`.
- **Загрузка отдельно от создания.** Контент загружается асинхронно, по лейблам и только через `IContentLoadingService`. Фабрики синхронные и берут префабы из кэша. Лейбл уровня освобождается при остановке игры.
- **Ленивые состояния.** Стейт-машина создаёт состояние при первом `Enter` через `IStateFactory<TStateMachine>` и кэширует его. У каждой машины своя фабрика: игровая, игрока, врага.
- **View / Handler.** View держит ссылки и сеттеры и отдаёт клики наружу событиями. Handler подписывается на эти события и вызывает доменную логику, например `GamePageView` + `GamePageHandler`.
- **Централизованные апдейты.** Объекты не используют Unity `Update`. Они регистрируются в `IUpdatingService` / `IFixedUpdatingService`, поэтому порядок апдейтов и время жизни контролирует фабрика.
- **Единый жизненный цикл.** `Initialize → Enter → … → Exit → Dispose` у базового `Component`, каскадно на дочерние компоненты.
- **DI.** `GlobalInstaller` (ProjectContext) держит сервисы на всю сессию. `DemoGameInstaller` (SceneContext сцены `Main`) держит объекты сцены. Доменные объекты (спавнеры, `BattleManager`, хендлеры) создаются через `new` внутри flow и фабрик.

## Структура

```
Assets/
  Core/
    Infrastructures/      базовые классы, интерфейсы, стейт-машина, AssetManagement
    Services/             реализации сервисов, UI-фабрика, базовые UI-вьюшки
    App/                  Bootstrap, GlobalInstaller, GameStateMachine и состояния
    Logic/                геймплей: Player, Enemy, Battle, Bullet, Gun, Pool, Spawner, ...
    Editor/               Tools/Addressables/Setup и Analyze
  AddressableAssets/      контент Addressables: Core, UI, Enemies, Levels/Level_N
  Configs/                конфиги (ScriptableObject)
  Resources/              ProjectContext, инсталлер конфигов, страницы загрузки и ошибки
  Scenes/                 Initialize (бутстрап), Main (игра)
```
