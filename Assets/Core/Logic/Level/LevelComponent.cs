using Core.Logic.Battle;
using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Level
{
    public sealed class LevelComponent : Component
    {
        [SerializeField] private Transform _playerPoint;
        [SerializeField] private BattlePlace[] _battlePlaces;

        public Transform PlayerPoint => _playerPoint;
        public BattlePlace[] BattlePlaces => _battlePlaces;
    }
}