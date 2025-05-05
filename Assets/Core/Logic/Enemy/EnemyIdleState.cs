using Core.Logic.Actor;

namespace Core.Logic.Enemy
{
    public sealed class EnemyIdleState : ActorIdleState<EnemyStateMachine, EnemyComponent>
    {
        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) { }
    }
}