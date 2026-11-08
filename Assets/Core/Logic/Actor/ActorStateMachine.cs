using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;

namespace Core.Logic.Actor
{
    public abstract class ActorStateMachine<TStateMachine, TActorComponent> : StateMachine<TStateMachine>
        where TStateMachine : ActorStateMachine<TStateMachine, TActorComponent>
        where TActorComponent : ActorComponent
    {
        public TActorComponent Actor { get; }

        protected ActorStateMachine(IStateFactory<TStateMachine> stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService, TActorComponent actor) :
            base(stateFactory, logger, updatingService, fixedUpdatingService) =>
            Actor = actor;
    }
}
