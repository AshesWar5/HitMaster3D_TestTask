using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;
using Core.Logic.Actor;

namespace Core.Logic.Enemy
{
    public sealed class EnemyStateMachine : ActorStateMachine<EnemyStateMachine, EnemyComponent>
    {
        public EnemyStateMachine(IStateFactory<EnemyStateMachine> stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService,
            EnemyComponent enemy) :
            base(stateFactory, logger, updatingService, fixedUpdatingService, enemy) { }
    }
}
