using Core.Infrastructures.Factories.Game;
using Core.Infrastructures.StateMachines;
using Core.Logic.Actor;
using Cysharp.Threading.Tasks;

namespace Core.Logic.Enemy
{
    public sealed class EnemyDeathState : ActorState<EnemyStateMachine, EnemyComponent>, IState
    {
        private readonly IGameFactory _gameFactory;
        public EnemyDeathState(EnemyStateMachine stateMachine, IGameFactory gameFactory) :
            base(stateMachine) => _gameFactory = gameFactory;

        protected override void OnDisposeInternal() =>
            _gameFactory.Remove(_actor);

        public async void Enter()
        {
            _actor.GetAnimation<EnemyAnimationHandler>().PlayDeath();
            await UniTask.Delay(1500);
            _stateMachine.Dispose();
        }

        public void Exit() { }
    }
}