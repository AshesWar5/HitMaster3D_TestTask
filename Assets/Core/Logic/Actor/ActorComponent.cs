using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Actor
{
    public abstract class ActorComponent : Component
    {
        [SerializeField] private Animator _animator;
        
        private ActorAnimationHandler _actorAnimation;

        public Animator Animator => _animator;

        public void SetAnimationHandler(ActorAnimationHandler handler) =>
            _actorAnimation = handler;

        public TAnimationHandler GetAnimation<TAnimationHandler>()
            where TAnimationHandler : ActorAnimationHandler =>
            _actorAnimation as TAnimationHandler;
    }
}