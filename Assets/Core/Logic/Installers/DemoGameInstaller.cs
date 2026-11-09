using Core.Infrastructures.AssetManagement;
using Core.Infrastructures.GameFlow;
using Core.Logic.Camera;
using Core.Logic.DemoGameFlow;
using Core.Logic.Factories;
using Core.Logic.Level;
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
            BindAssetInstantiator();
            BindGameFactory();
            BindLevelSelector();
            BindGameFlowManager();
        }

        private void BindGameFactory()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
        }
        
        private void BindLevelSelector()
        {
            Container.Bind<ILevelSelector>().To<RandomLevelSelector>().AsSingle();
        }

        private void BindAssetInstantiator()
        {
            Container
                .Bind<IAssetInstantiator>()
                .To<AssetInstantiator>()
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