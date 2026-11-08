using System.Collections.Generic;
using Core.Infrastructures.Factories.Game;
using Core.Logic.Enemy;
using UnityEngine;

namespace Core.Logic.Battle
{
    public sealed class BattlePreparation
    {
        private readonly IGameFactory _gameFactory;
        private readonly BattlePlace[] _places;
        private readonly List<BattlePlaceData> _placeDatas;

        private bool _isPreparedForBattle;

        public BattlePreparation(IGameFactory gameFactory, BattlePlace[] places)
        {
            _gameFactory = gameFactory;
            _places = places;
            _placeDatas = new List<BattlePlaceData>();
        }

        public IEnumerable<BattlePlaceData> Prepare()
        {
            if(_isPreparedForBattle)
                return null;

            int lastNumberBattlePlace = 0;
            
            foreach (BattlePlace place in _places)
            {
                var data = new BattlePlaceData(lastNumberBattlePlace,
                    place.CenterPositionBattlePoint.position);
                
                if (place.CountSpawnEnemies > 0)
                {
                    for (int i = 0; i < place.CountSpawnEnemies; i++)
                    {
                        int indexEnemy = Random.Range(0, place.Enemies.Length);

                        var instance = _gameFactory.CreateEnemy<EnemyComponent>(place.Enemies[indexEnemy],
                            place.PointsForSpawn[i].position, lastNumberBattlePlace);
                        
                        data.AddEnemy(instance);
                    }
                }
                
                _placeDatas.Add(data);

                lastNumberBattlePlace++;
            }

            _isPreparedForBattle = true;

            return _placeDatas;
        }

        public void Clear()
        {
            foreach (BattlePlaceData data in _placeDatas)
            {
                foreach (EnemyComponent enemy in data.Enemies)
                {
                    if(enemy == null)
                        continue;

                    _gameFactory.Remove(enemy);
                }
            }

            _placeDatas.Clear();
            _isPreparedForBattle = false;
        }
    }
}