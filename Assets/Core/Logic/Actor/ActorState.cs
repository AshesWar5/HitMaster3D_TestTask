using Core.Infrastructures.StateMachines;

namespace Core.Logic.Actor
{
    public abstract class ActorState<TStateMachine, TActorComponent> : State<TStateMachine>
        where TStateMachine : ActorStateMachine<TStateMachine, TActorComponent>
        where TActorComponent : ActorComponent
    {
        protected readonly TActorComponent _actor;

        protected ActorState(TStateMachine stateMachine) : base(stateMachine) =>
            _actor = stateMachine.Actor;
    }
}