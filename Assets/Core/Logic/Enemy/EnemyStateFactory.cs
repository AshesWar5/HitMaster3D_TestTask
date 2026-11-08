using System;
using Core.Infrastructures.StateMachines;
using Core.Logic.Factories;

namespace Core.Logic.Enemy
{
    public sealed class EnemyStateFactory : IStateFactory<EnemyStateMachine>
    {
        private readonly IGameFactory _gameFactory;

        public EnemyStateFactory(IGameFactory gameFactory) =>
            _gameFactory = gameFactory;

        public TState CreateState<TState>(EnemyStateMachine stateMachine) where TState : IExitableState
        {
            IExitableState state = typeof(TState) switch
            {
                Type type when type == typeof(EnemyIdleState) => new EnemyIdleState(stateMachine),
                Type type when type == typeof(EnemyDeathState) => new EnemyDeathState(stateMachine, _gameFactory),
                _ => throw new ArgumentOutOfRangeException(nameof(TState), typeof(TState), null)
            };

            return (TState)state;
        }
    }
}
