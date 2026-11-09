**English** | [Русский](README.ru.md)

# HitMaster3D Test Task

A hit-master style shooter prototype. The player runs between battle points and shoots enemies by tap or click. When the player clears a point, they move on to the next one. After the last point the level restarts.

![Gameplay](Docs/Gameplay.gif)

**Stack:** Unity 6 (6000.4.3f1), URP, Zenject (Extenject), UniTask, Addressables.

## Running

- **In the editor:** open `Assets/Scenes/Initialize.unity` and press Play. Do not start from `Main`: services and content are loaded in `Initialize`.
- **Build:** first build the Addressables content (`Window → Asset Management → Addressables → Groups → Build → New Build → Default Build Script`), then build the player.
- **After adding or moving content:** run `Tools/Addressables/Setup`, then `Tools/Addressables/Analyze`. Setup creates the groups, addresses and labels. Do not edit them by hand.

## Layers

All project code lives in `Assets/Core`. Each layer is a separate assembly (asmdef), so the compiler enforces the dependency direction.

```
Infrastructures  ←  Services  ←  App
       ↑               ↑
       └──── Logic ────┘
```

| Layer | Contents |
|---|---|
| `Infrastructures` | Base classes and abstractions: `Component`/`View`, state machine, service interfaces, `IGameFlow`, `IGameRestart`. Depends on nothing. |
| `Services` | Global service implementations: input, pages and UI factory, scene loading, updates, content loading. |
| `App` | Application root: `Bootstrap`, `GlobalInstaller`, `GameStateMachine` and its states. |
| `Logic` | Gameplay, one folder per feature: player, enemies, battle, bullets, pool, spawners, camera, game factory, scene installers. |

`App` and `Logic` do not know about each other. The application starts the scene through `IGameFlow`, and the scene asks for a restart through `IGameRestart`.

## Startup flow

```
Bootstrap → BootstrapState → LoadContentState → LoadLevelState → GameLoopState → IGameFlow
```

1. `Bootstrap` is the entry point on the `Initialize` scene. It starts `GameStateMachine`.
2. `LoadContentState` loads the `core` and `ui` labels once per session, showing the loading page. If loading fails, it shows an error page with Retry.
3. `LoadLevelState` loads the `Main` scene and takes `IGameFlow` from its `SceneContext`.
4. `GameLoopState` awaits `StartGame()` and forwards `Updating`/`FixedUpdating` to the flow.
5. `DemoGameFlowManager` picks a level, loads its `level_N` label and builds the level: level → player → camera → battle points → `BattleManager` → game page.

## Key decisions

- **Restart through an abstraction.** `BattleManager` only raises `Completed`. `DemoGameFlowManager` decides to restart and calls `IGameRestart`. `GameRestartService` in `App` disposes `GameStateMachine` and re-enters `BootstrapState` on the next frame.
- **Loading is separate from creation.** Content is loaded asynchronously, by label, only through `IContentLoadingService`. Factories are synchronous and take prefabs from the cache. A level's label is released when the game stops.
- **Lazy states.** A state machine creates a state on its first `Enter` through `IStateFactory<TStateMachine>` and caches it. Each machine has its own factory: game, player, enemy.
- **View / Handler.** A View holds references and setters and exposes clicks as events. A Handler subscribes to those events and calls the domain logic, for example `GamePageView` + `GamePageHandler`.
- **Centralized updates.** Objects do not use Unity `Update`. They register in `IUpdatingService` / `IFixedUpdatingService`, so the factory controls the update order and lifetime.
- **Single object lifecycle.** `Initialize → Enter → … → Exit → Dispose` on the `Component` base class, cascading to child components.
- **DI.** `GlobalInstaller` (ProjectContext) holds services for the whole session. `DemoGameInstaller` (SceneContext of `Main`) holds the scene's objects. Domain objects (spawners, `BattleManager`, handlers) are created with `new` inside the flow and factories.

## Improvements after the test task

- **Layers as assemblies.** Code is split into `Infrastructures` / `Services` / `App` / `Logic` asmdefs, and dependencies go one way only. Game-specific factories and installers moved out of `Infrastructures` into `Logic`.
- **Addressables.** Content is split into groups and labels and preloaded behind a loading page, with an error page and Retry on failure. `Tools/Addressables/Setup` and `Analyze` build and check the groups.
- **Restart outside the domain.** `BattleManager` no longer restarts the game itself. `GameRestartService` does it through `IGameRestart`.
- **State factories.** States are created lazily through a separate factory for each state machine instead of being registered up front.
- **Bug fixes.** Bullet pooling, enemy cleanup on restart, mobile input, and cancelling the enemy removal delay when the enemy is disposed.
- **Code cleanup.** Empty `catch` blocks were replaced with destroyed object checks, `async void` was replaced with UniTask, actor dependencies are passed through one `Setup` call, and the project is upgraded to Unity 6.

## Structure

```
Assets/
  Core/
    Infrastructures/      base classes, interfaces, state machine, AssetManagement
    Services/             service implementations, UI factory, base UI views
    App/                  Bootstrap, GlobalInstaller, GameStateMachine and states
    Logic/                gameplay: Player, Enemy, Battle, Bullet, Gun, Pool, Spawner, ...
    Editor/               Tools/Addressables/Setup and Analyze
  AddressableAssets/      Addressables content: Core, UI, Enemies, Levels/Level_N
  Configs/                configs (ScriptableObject)
  Resources/              ProjectContext, configs installer, loading and error pages
  Scenes/                 Initialize (bootstrap), Main (game)
```
