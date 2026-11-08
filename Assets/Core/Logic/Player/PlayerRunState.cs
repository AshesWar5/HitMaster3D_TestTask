using Core.Infrastructures;
using Core.Infrastructures.StateMachines;
using Core.Logic.Actor;
using UnityEngine;

namespace Core.Logic.Player
{
    public sealed class PlayerRunState : ActorState<PlayerStateMachine, PlayerComponent>, IPayLoadedState<Vector3>
    {
        private Vector3 _currentTargetPosition;
        
        public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public void Enter(Vector3 targetPosition)
        {
            _currentTargetPosition = targetPosition;
            
            _actor.SetDestination(targetPosition);
            _actor.Animation.PlayRun();
        }

        protected override void OnUpdatingInternal()
        {
            if (_actor.CurrentPositionPlayer.DistanceOptimize(_currentTargetPosition, 1))
            {
                _actor.AchievedLastTargetMovement();
            }
        }

        public void Exit() { }
    }
}