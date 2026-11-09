using Core.Infrastructures.AssetManagement;
using Core.Logic.Gun;
using UnityEngine;

namespace Core.Logic.Player
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Player/Config", order = 0)]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField] private float _scale = 1;
        [SerializeField] private float _speed = 10;
        [SerializeField] private ComponentReference<PlayerComponent> _prefab;
        [SerializeField] private GunConfig _gun;

        public float Scale => _scale;
        public float Speed => _speed;
        public ComponentReference<PlayerComponent> Prefab => _prefab;
        public GunConfig Gun => _gun;
    }
}