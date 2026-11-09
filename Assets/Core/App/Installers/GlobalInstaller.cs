using Core.App.GameFlow;
using Core.App.StateMachines;
using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.GameFlow;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Services.ContentLoading;
using Core.Services.Input;
using Core.Services.Page;
using Core.Services.SceneLoader;
using Core.Services.UI;
using Core.Services.Update;
using UnityEngine;
using Zenject;

namespace Core.App.Installers
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
            BindAssetInstantiator();
            BindUIFactory();
            BindPageController();
            BindContentLoadingService();
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

        private void BindAssetInstantiator()
        {
            Container
                .Bind<IAssetInstantiator>()
                .To<AssetInstantiator>()
                .AsSingle();
        }

        private void BindStateMachineFactory()
        {
            Container
                .Bind<IStateFactory<GameStateMachine>>()
                .To<GameStateFactory>()
                .AsSingle();
        }

        private void BindGameStateMachine()
        {
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

        private void BindContentLoadingService()
        {
            Container.Bind<IContentLoadingService>().To<ContentLoadingService>().AsSingle();
        }
    }
}