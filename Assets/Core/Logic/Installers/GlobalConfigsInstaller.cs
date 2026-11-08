using Core.Logic.Level;
using Core.Logic.Player;
using UnityEngine;
using Zenject;

namespace Core.Logic.Installers
{
    [CreateAssetMenu(fileName = "Global Configs Installer",
        menuName = "Installers/Configs/Global")]
    public sealed class GlobalConfigsInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private LevelsConfig _levelsConfig;
        [SerializeField] private PlayerConfig _player;

        public override void InstallBindings()
        {
            Container.BindInstance(_levelsConfig);
            Container.BindInstance(_player);
        }
    }
}