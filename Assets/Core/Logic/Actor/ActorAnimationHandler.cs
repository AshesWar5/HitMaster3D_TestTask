using UnityEngine;

namespace Core.Logic.Actor
{
    public abstract class ActorAnimationHandler
    {
        protected readonly Animator _animator;

        protected ActorAnimationHandler(Animator animator) =>
            _animator = animator;

        public abstract void PlayIdle();
    }
}