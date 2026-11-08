using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Actor
{
    public abstract class ActorComponent : Component
    {
        [SerializeField] private Animator _animator;

        public Animator Animator => _animator;
        public abstract ActorAnimationHandler ActorAnimation { get; }
    }

    public abstract class ActorComponent<TAnimationHandler> : ActorComponent
        where TAnimationHandler : ActorAnimationHandler
    {
        private TAnimationHandler _animation;

        public TAnimationHandler Animation => _animation;
        public override ActorAnimationHandler ActorAnimation => _animation;

        public void SetAnimationHandler(TAnimationHandler handler) =>
            _animation = handler;
    }
}
