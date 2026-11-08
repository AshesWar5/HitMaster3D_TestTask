using System;
using System.Collections.Generic;
using Core.Infrastructures;
using Core.Logic.Enemy;
using Core.Logic.Player;

namespace Core.Logic.Battle
{
    public sealed class BattleManager : IInitialize, IDisposable, IBattleStart
    {
        public event Action Completed;

        private readonly IPlayerBattle _playerBattle;
        private readonly Dictionary<int, BattlePlaceData> _commands;

        private int _currentIdBattlePlace;

        public BattleManager(IPlayerBattle playerBattle, IEnumerable<BattlePlaceData> placeDatas)
        {
            _playerBattle = playerBattle;

            _commands = new Dictionary<int, BattlePlaceData>();
            foreach (BattlePlaceData data in placeDatas)
            {
                _commands.Add(data.IdBattlePlace, data);
            }
        }

        public void Initialize()
        {
            _currentIdBattlePlace = 0;
            
            _playerBattle.EndTargetMovement += OnEndTargetPlayerMovement;
            
            if (_commands.Count > 0)
            {
                foreach (BattlePlaceData data in _commands.Values)
                {
                    if(data.Enemies.Count <= 0)
                        continue;

                    foreach (EnemyComponent enemy in data.Enemies)
                    {
                        enemy.Death += OnDeathEnemy;
                    }
                }
            }
        }

        public void Dispose()
        {
            _playerBattle.EndTargetMovement -= OnEndTargetPlayerMovement;
            
            if (_commands.Count > 0)
            {
                foreach (BattlePlaceData data in _commands.Values)
                {
                    if(data.Enemies.Count <= 0)
                        continue;

                    foreach (EnemyComponent enemy in data.Enemies)
                    {
                        if(enemy == null)
                            continue;
                        
                        enemy.Death -= OnDeathEnemy;
                    }
                }
            }
            
            _commands.Clear();
        }

        public void StartBattle() => IsFinishCurrentBattlePlace();

        private bool IsFinishCurrentBattlePlace()
        {
            _commands.TryGetValue(_currentIdBattlePlace, out BattlePlaceData data);
            if (data is not null)
            {
                if (data.Enemies.Count <= 0)
                {
                    if (TryToMoveNextBattlePlace())
                    {
                        _playerBattle.Move(data.CenterPositionBattlePoint);
                    }

                    return true;
                }

                return false;
            }

            Completed?.Invoke();

            return true;
        }

        private bool TryToMoveNextBattlePlace()
        {
            if (_commands.Count >= _currentIdBattlePlace + 1)
            {
                _currentIdBattlePlace++;

                return true;
            }

            return false;
        }

        private void OnDeathEnemy(EnemyComponent enemy)
        {
            enemy.Death -= OnDeathEnemy;

            _commands.TryGetValue(enemy.IdBattlePlace, out BattlePlaceData data);
            if (data is not null)
            {
                data.RemoveEnemy(enemy);
                IsFinishCurrentBattlePlace();
            }
        }
        
        private void OnEndTargetPlayerMovement()
        {
            if (!IsFinishCurrentBattlePlace())
            {
                _playerBattle.StartBattle();
            }
        }
    }
}