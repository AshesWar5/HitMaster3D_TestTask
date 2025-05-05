using Core.Logic.Actor;
using UnityEngine;

namespace Core.Logic.Player
{
    public sealed class PlayerAnimationHandler : ActorAnimationHandler
    {
        private const string KEY_IDLE = "Idle";
        private const string KEY_RUN = "Run";
        
        private readonly int _indexIdleKey;
        private readonly int _indexRunKey;

        public PlayerAnimationHandler(Animator animator) : base(animator)
        { 
            _indexIdleKey = Animator.StringToHash(KEY_IDLE);
            _indexRunKey = Animator.StringToHash(KEY_RUN);
        }
        
        public override void PlayIdle() =>
            _animator.SetBool(_indexRunKey, false);

        public void PlayRun() =>
            _animator.SetBool(_indexRunKey, true);
    }
}