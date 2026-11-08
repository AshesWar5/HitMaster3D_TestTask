using System.Threading;
using Core.Infrastructures.StateMachines;
using Core.Logic.Actor;
using Core.Logic.Factories;
using Cysharp.Threading.Tasks;

namespace Core.Logic.Enemy
{
    public sealed class EnemyDeathState : ActorState<EnemyStateMachine, EnemyComponent>, IState
    {
        private const int DELAY_REMOVE_MS = 1500;

        private readonly IGameFactory _gameFactory;

        private CancellationTokenSource _cancellationTokenSource;

        public EnemyDeathState(EnemyStateMachine stateMachine, IGameFactory gameFactory) :
            base(stateMachine) => _gameFactory = gameFactory;

        public async void Enter()
        {
            _actor.Animation.PlayDeath();

            _cancellationTokenSource = new CancellationTokenSource();
            bool isCanceled = await UniTask.Delay(DELAY_REMOVE_MS,
                cancellationToken: _cancellationTokenSource.Token).SuppressCancellationThrow();

            if(isCanceled)
                return;

            _gameFactory.Remove(_actor);
        }

        public void Exit() =>
            CancelRemove();

        private void CancelRemove()
        {
            if(_cancellationTokenSource is null)
                return;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        protected override void OnDisposeInternal() =>
            CancelRemove();
    }
}
