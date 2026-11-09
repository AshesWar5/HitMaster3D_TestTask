using Core.Infrastructures.AssetManagement;
using UnityEngine;

namespace Core.Logic.Bullet
{
    [CreateAssetMenu(fileName = "Bullet Config", menuName = "Bullet/Config", order = 0)]
    public sealed class BulletConfig : ScriptableObject
    {
        [SerializeField] private float _scale = 1;
        [SerializeField] private float _damage = 1;
        [SerializeField] private ComponentReference<BulletComponent> _prefab;

        public float Scale => _scale;
        public float Damage => _damage;
        public ComponentReference<BulletComponent> Prefab => _prefab;
    }
}