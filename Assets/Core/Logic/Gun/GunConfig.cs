using Core.Infrastructures.AssetManagement;
using Core.Logic.Bullet;
using UnityEngine;

namespace Core.Logic.Gun
{
    [CreateAssetMenu(fileName = "Gun Config", menuName = "Gun/Config", order = 0)]
    public sealed class GunConfig : ScriptableObject
    {
        [SerializeField] private float _scale = 1;
        [SerializeField, Tooltip("This is the speed at which the cartridge flies.")]
        private float _speedBullet;
        [SerializeField] private ComponentReference<GunComponent> _prefab;
        [SerializeField] private BulletConfig _bulletConfig;

        public float Scale => _scale;
        public float SpeedBullet => _speedBullet;
        public ComponentReference<GunComponent> Prefab => _prefab;
        public BulletConfig BulletConfig => _bulletConfig;
    }
}