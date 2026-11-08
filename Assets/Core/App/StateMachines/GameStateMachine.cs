using System;
using System.Collections.Generic;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;

namespace Core.App.StateMachines
{
    public sealed class GameStateMachine : StateMachine<GameStateMachine>
    {
        public GameStateMachine(IStateFactory stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService)
            : base(stateFactory, logger, updatingService, fixedUpdatingService)
        {
            _states = new Dictionary<Type, State<GameStateMachine>>
            {
                [typeof(BootstrapState)] = null,
                [typeof(LoadLevelState)] = null,
                [typeof(GameLoopState)] = null
            };
        }
    }
}