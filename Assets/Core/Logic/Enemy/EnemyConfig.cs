using UnityEngine;

namespace Core.Logic.Enemy
{
    [CreateAssetMenu(fileName = "Enemy Config", menuName = "Enemy/Config", order = 0)]
    public sealed class EnemyConfig : ScriptableObject
    {
        [SerializeField] private float _scale = 1;
        [SerializeField] private float _health = 10;
        [SerializeField] private EnemyComponent _prefab;
        
        public float Scale => _scale;
        public float Health => _health;
        public EnemyComponent Prefab => _prefab;
    }
}