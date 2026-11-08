using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;

namespace Core.Logic.Actor
{
    public abstract class ActorStateMachine<TStateMachine, TActorComponent> : StateMachine<TStateMachine>
        where TStateMachine : ActorStateMachine<TStateMachine, TActorComponent>
        where TActorComponent : ActorComponent
    {
        public TActorComponent Actor { get; protected set; }

        protected ActorStateMachine(IStateFactory stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService) :
            base(stateFactory, logger, updatingService, fixedUpdatingService) { }
    }
}