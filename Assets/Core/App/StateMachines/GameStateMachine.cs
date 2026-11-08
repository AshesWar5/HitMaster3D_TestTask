using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;

namespace Core.App.StateMachines
{
    public sealed class GameStateMachine : StateMachine<GameStateMachine>
    {
        public GameStateMachine(IStateFactory<GameStateMachine> stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService)
            : base(stateFactory, logger, updatingService, fixedUpdatingService) { }
    }
}