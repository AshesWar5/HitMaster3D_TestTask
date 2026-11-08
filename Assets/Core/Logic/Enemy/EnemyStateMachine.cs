using System;
using System.Collections.Generic;
using Core.Infrastructures.Logger;
using Core.Infrastructures.StateMachines;
using Core.Infrastructures.Update;
using Core.Logic.Actor;
using Core.Logic.Factories;

namespace Core.Logic.Enemy
{
    public sealed class EnemyStateMachine : ActorStateMachine<EnemyStateMachine, EnemyComponent>
    {
        public EnemyStateMachine(IStateFactory stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService,
            EnemyComponent enemyComponent, IGameFactory gameFactory) :
            base(stateFactory, logger, updatingService, fixedUpdatingService)
        {
            Actor = enemyComponent;
            
            _states = new Dictionary<Type, State<EnemyStateMachine>>
            {
                [typeof(EnemyIdleState)] = new EnemyIdleState(this),
                [typeof(EnemyDeathState)] = new EnemyDeathState(this, gameFactory)
            };
        }
    }
}