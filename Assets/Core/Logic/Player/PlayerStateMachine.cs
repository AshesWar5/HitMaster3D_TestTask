using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;
using Core.Logic.Actor;

namespace Core.Logic.Player
{
    public sealed class PlayerStateMachine : ActorStateMachine<PlayerStateMachine, PlayerComponent>
    {
        public PlayerStateMachine(IStateFactory<PlayerStateMachine> stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService,
            PlayerComponent player) :
            base(stateFactory, logger, updatingService, fixedUpdatingService, player) { }
    }
}