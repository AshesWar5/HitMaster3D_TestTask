using Core.Logic.Actor;
using UnityEngine;

namespace Core.Logic.Enemy
{
    public sealed class EnemyAnimationHandler : ActorAnimationHandler
    {
        private const string KEY_IDLE = "Idle";
        private const string KEY_DEATH = "Death";
        
        private readonly int _indexIdleKey;
        private readonly int _indexDeathKey;

        public EnemyAnimationHandler(Animator animator) : base(animator)
        {
            _indexIdleKey = Animator.StringToHash(KEY_IDLE);
            _indexDeathKey = Animator.StringToHash(KEY_DEATH);
        }

        public override void PlayIdle() { }

        public void PlayDeath() =>
            _animator.SetTrigger(_indexDeathKey);
    }
}