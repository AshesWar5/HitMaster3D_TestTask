using Core.Logic.Enemy;
using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Battle
{
    public sealed class BattlePlace : Component
    {
        [SerializeField] private Transform[] _pointsForSpawn;
        [SerializeField] private EnemyConfig[] _enemies;
        [SerializeField] private Transform _centerPositionBattlePoint;

        public int CountSpawnEnemies => _pointsForSpawn.Length;
        public Transform[] PointsForSpawn => _pointsForSpawn;
        public EnemyConfig[] Enemies => _enemies;
        public Transform CenterPositionBattlePoint => _centerPositionBattlePoint;
    }
}