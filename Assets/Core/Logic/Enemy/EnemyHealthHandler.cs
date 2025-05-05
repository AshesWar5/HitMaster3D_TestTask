using Core.Infrastructures;
using Core.Logic.Actor;

namespace Core.Logic.Enemy
{
    public class EnemyHealthHandler : ActorHealth, IInitialize
    {
        private readonly EnemyHealthView _view;

        public EnemyHealthHandler(float health, EnemyHealthView view) : base(health) =>
            _view = view;

        public void Initialize()
        {
            _view.SetHealthMin(0);
            _view.SetHealthMax(_health);
            _view.SetCurrentHealth(_health);
        }

        public override void SetDamage(float damage)
        {
            base.SetDamage(damage);
            _view.SetCurrentHealth(_health);
        }
    }
}