using System;
using Core.Logic.Health;

namespace Core.Logic.Actor
{
    public class ActorHealth : IHealth
    {
        public event Action Death;

        protected float _health { get; private set; }

        public float Health => _health;

        public ActorHealth(float health) =>
            _health = health;

        public virtual void SetDamage(float damage)
        {
            if (_health > 0)
            {
                _health -= damage;

                CheckDeath();
            }
        }

        protected virtual void CheckDeath()
        {
            if (_health <= 0)
            {
                Death?.Invoke();
            }
        }
    }
}