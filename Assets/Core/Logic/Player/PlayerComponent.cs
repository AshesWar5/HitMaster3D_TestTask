using System;
using Core.Logic.Actor;
using Core.Logic.Gun;
using UnityEngine;
using UnityEngine.AI;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Player
{
    public sealed class PlayerComponent : ActorComponent<PlayerAnimationHandler>, IPlayerBattle
    {
        public event Action EndTargetMovement;
        
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private Transform _pointSpawnGun;
        
        private PlayerStateMachine _stateMachine;
        private GunComponent _gun;
        
        public Vector3 CurrentPositionPlayer => transform.position;
        public Transform PointSpawnGun => _pointSpawnGun;
        public Component Gun => _gun;

        protected override void OnInitialize() =>
            _stateMachine.Enter<PlayerIdleState>();

        protected override void OnDispose() =>
            _stateMachine.Dispose();

        public void SetStateMachine(PlayerStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void SetSpeed(float speed) =>
            _navMeshAgent.speed = speed;

        public void SetGun(GunComponent gun) =>
            _gun = gun;

        public void Move(Vector3 targetPosition) =>
            _stateMachine.Enter<PlayerRunState, Vector3>(targetPosition);

        public void StartBattle() =>
            _stateMachine.Enter<PlayerBattleState>();

        public void AchievedLastTargetMovement() =>
            EndTargetMovement?.Invoke();

        public void SetDestination(Vector3 targetPosition) =>
            _navMeshAgent.SetDestination(targetPosition);
    }
}