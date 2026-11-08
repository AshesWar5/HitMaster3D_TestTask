using System;
using System.Collections.Generic;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;
using Core.Logic.Actor;

namespace Core.Logic.Player
{
    public sealed class PlayerStateMachine : ActorStateMachine<PlayerStateMachine, PlayerComponent>
    {
        public PlayerStateMachine(IStateFactory stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService,
            PlayerComponent player) : base(stateFactory, logger, updatingService, fixedUpdatingService)
        {
            Actor = player;
            
            _states = new Dictionary<Type, State<PlayerStateMachine>>
            {
                [typeof(PlayerIdleState)] = new PlayerIdleState(this),
                [typeof(PlayerRunState)] = new PlayerRunState(this),
                [typeof(PlayerBattleState)] = new PlayerBattleState(this)
            };
        }
    }
}