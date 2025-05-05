using System.Collections.Generic;
using Core.Logic.Enemy;
using UnityEngine;

namespace Core.Logic.Battle
{
    public sealed class BattlePlaceData
    {
        public readonly int IdBattlePlace;
        public readonly Vector3 CenterPositionBattlePoint;
        
        private List<EnemyComponent> _enemies;

        public IReadOnlyList<EnemyComponent> Enemies => _enemies;
        
        public BattlePlaceData(int idBattlePlace, IEnumerable<EnemyComponent> enemies,
            Vector3 centerPositionBattlePoint)
        {
            IdBattlePlace = idBattlePlace;
            CenterPositionBattlePoint = centerPositionBattlePoint;
            _enemies = new List<EnemyComponent>(enemies);
        }
        
        public BattlePlaceData(int idBattlePlace, Vector3 centerPositionBattlePoint)
        {
            IdBattlePlace = idBattlePlace;
            CenterPositionBattlePoint = centerPositionBattlePoint;
            _enemies = new List<EnemyComponent>();
        }
        
        ~BattlePlaceData() => _enemies = null;

        public void AddEnemy(EnemyComponent enemy)
        {
            if(_enemies.Contains(enemy))
                return;
            
            _enemies.Add(enemy);
        }

        public void RemoveEnemy(EnemyComponent enemy)
        {
            if(!_enemies.Contains(enemy))
                return;
            
            _enemies.Remove(enemy);
        }
    }
}