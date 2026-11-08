using Core.Infrastructures.StateMachines;
using Core.Logic.Actor;

namespace Core.Logic.Player
{
    public sealed class PlayerBattleState : ActorState<PlayerStateMachine, PlayerComponent>, IState
    {
        public PlayerBattleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

        public void Enter()
        {
            _actor.Gun.Show();
            _actor.Animation.PlayIdle();
        }

        public void Exit() => _actor.Gun.Hide();
    }
}