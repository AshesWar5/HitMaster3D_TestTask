using System;
using Core.Logic.Actor;
using Core.Logic.Health;

namespace Core.Logic.Enemy
{
    public class EnemyComponent : ActorComponent<EnemyAnimationHandler>, IHealth
    {
        public event Action<EnemyComponent> Death;
        
        private EnemyStateMachine _stateMachine;
        private ActorHealth _health;
        private int _idBattlePlace;

        public int IdBattlePlace => _idBattlePlace;

        protected override void OnInitialize()
        {
            _stateMachine.Enter<EnemyIdleState>();
            
            _health.Death += OnDeath;
        }

        protected override void OnDispose()
        {
            _health.Death -= OnDeath;
            _stateMachine.Dispose();
        }
        
        public void SetStateMachine(EnemyStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void SetIdBattlePlace(int id) =>
            _idBattlePlace = id;

        public void SetHealth(ActorHealth health) =>
            _health = health;

        public void SetDamage(float damage) =>
            _health.SetDamage(damage);

        private void OnDeath()
        {
            _stateMachine.Enter<EnemyDeathState>();
            Death?.Invoke(this);
        }
    }
}