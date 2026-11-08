using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.GameFlow;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.StateMachines.Game;
using Core.Services.Input;
using Core.Services.Page;
using Core.Services.SceneLoader;
using Core.Services.UI;
using Core.Services.Update;
using UnityEngine;
using Zenject;

namespace Core.Infrastructures.Installers
{
    public sealed class GlobalInstaller : MonoInstaller
    { 
        public override void InstallBindings()
        {
            BindLoggerService();
            BindUpdateServices();
            BindInputService();
            BindLoaderScene();
            BindAssetProvider();
            BindUIFactory();
            BindPageController();
            BindStateMachineFactory();
            BindGameStateMachine();
            BindGameRestart();
        }

        private void BindUpdateServices()
        {
            Container.BindInterfacesAndSelfTo<UpdatingService>().AsSingle();
            Container.BindInterfacesAndSelfTo<FixedUpdatingService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LateUpdatingService>().AsSingle();
        }

        private void BindInputService()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                Container.BindInterfacesAndSelfTo<MobileInputService>().AsSingle();
            }
            else
            {
                Container.BindInterfacesAndSelfTo<StandaloneInputService>().AsSingle();
            }
        }

        private void BindGameRestart()
        {
            Container.Bind<IGameRestart>().To<GameRestartService>().AsSingle();
        }

        private void BindLoggerService()
        {
            Container.Bind<ILoggerService>().To<LoggerService>().AsSingle();
        }

        private void BindLoaderScene()
        {
            Container.Bind<ISceneLoaderService>().To<SceneLoaderService>().AsSingle();
        }

        private void BindAssetProvider()
        {
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();
        }

        private void BindStateMachineFactory()
        {
            Container
                .Bind<IStateFactory>()
                .To<StateFactory>()
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
            Container.Bind<BootstrapState>().AsSingle();
            Container.Bind<LoadLevelState>().AsSingle();
            Container.Bind<GameLoopState>().AsSingle();

            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }
        
        private void BindUIFactory()
        {
            Container.BindInterfacesAndSelfTo<UIFactory>().AsSingle();
        }

        private void BindPageController()
        {
            Container.Bind<IPageService>().To<PageService>().AsSingle();
        }
    }
}