using System;
using Core.Infrastructures.StateMachines;

namespace Core.Logic.Player
{
    public sealed class PlayerStateFactory : IStateFactory<PlayerStateMachine>
    {
        public TState CreateState<TState>(PlayerStateMachine stateMachine) where TState : IExitableState
        {
            IExitableState state = typeof(TState) switch
            {
                Type type when type == typeof(PlayerIdleState) => new PlayerIdleState(stateMachine),
                Type type when type == typeof(PlayerRunState) => new PlayerRunState(stateMachine),
                Type type when type == typeof(PlayerBattleState) => new PlayerBattleState(stateMachine),
                _ => throw new ArgumentOutOfRangeException(nameof(TState), typeof(TState), null)
            };

            return (TState)state;
        }
    }
}
