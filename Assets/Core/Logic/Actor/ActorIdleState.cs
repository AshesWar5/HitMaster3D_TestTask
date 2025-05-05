using Core.Infrastructures.StateMachines;

namespace Core.Logic.Actor
{
    public class ActorIdleState<TStateMachine, TActorComponent> : ActorState<TStateMachine, TActorComponent>, IState
        where TStateMachine : ActorStateMachine<TStateMachine, TActorComponent>
        where TActorComponent : ActorComponent
    {
        protected ActorIdleState(TStateMachine stateMachine) : base(stateMachine) { }

        public virtual void Enter() =>
            _actor.GetAnimation<ActorAnimationHandler>().PlayIdle();

        public virtual void Exit() { }
    }
}