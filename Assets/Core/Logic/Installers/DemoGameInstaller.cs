using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.GameFlow;
using Core.Infrastructures.StateMachines;
using Core.Logic.Camera;
using Core.Logic.DemoGameFlow;
using Core.Logic.Factories;
using UnityEngine;
using Zenject;

namespace Core.Logic.Installers
{
    public class DemoGameInstaller : MonoInstaller
    {
        [SerializeField] private CameraFollow _cameraFollow;
        
        public override void InstallBindings()
        {
            BindCameraFollow();
            BindAssetProvider();
            BindGameFactory();
            BindStateMachineFactory();
            BindGameFlowManager();
        }

        private void BindStateMachineFactory()
        {
            Container
                .Bind<IStateFactory>()
                .To<StateFactory>()
                .AsSingle();
        }

        private void BindGameFactory()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
        }
        
        private void BindAssetProvider()
        {
            Container
                .Bind<IAssetProvider>()
                .To<AssetProvider>()
                .AsSingle();
        }

        private void BindCameraFollow()
        {
            Container.BindInstance(_cameraFollow);
        }

        private void BindGameFlowManager()
        {
            Container.Bind<IGameFlow>().To<DemoGameFlowManager>().AsSingle();
        }
    }
}