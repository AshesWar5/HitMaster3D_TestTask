using Core.Logic.Actor;

namespace Core.Logic.Player
{
    public sealed class PlayerIdleState : ActorIdleState<PlayerStateMachine, PlayerComponent>
    {
        public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _actor.Gun.Hide();
        }
    }
}