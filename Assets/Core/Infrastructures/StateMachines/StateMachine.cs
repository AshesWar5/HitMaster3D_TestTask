using System;
using System.Collections.Generic;
using Core.Infrastructures.Logger;
using Core.Infrastructures.Update;

namespace Core.Infrastructures.StateMachines
{
    public abstract class StateMachine<FSM> : IStateMachine<FSM> where FSM : class, IStateMachine<FSM>
    {
        private readonly IStateFactory<FSM> _stateFactory;
        private readonly ILoggerService _logger;
        private readonly IUpdatingService _updatingService;
        private readonly IFixedUpdatingService _fixedUpdatingService;
        private readonly Dictionary<Type, State<FSM>> _states;
        private IExitableState _activeState;
        private State<FSM> _currentState;

        protected StateMachine(IStateFactory<FSM> stateFactory, ILoggerService logger,
            IUpdatingService updatingService, IFixedUpdatingService fixedUpdatingService)
        {
            _stateFactory = stateFactory;
            _logger = logger;
            _updatingService = updatingService;
            _fixedUpdatingService = fixedUpdatingService;
            _states = new Dictionary<Type, State<FSM>>();
        }

        public void Enter<TState>() where TState : State<FSM>, IState
        {
            var state = ChangeState<TState>();
            state.Initialize();
            state.Enter();
            _updatingService.Register(state);
            _fixedUpdatingService.Register(state);
        }
        
        public void Enter<TState, TPayload>(TPayload payload)
            where TState : State<FSM>, IPayLoadedState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Initialize();
            state.Enter(payload);
            _updatingService.Register(state);
            _fixedUpdatingService.Register(state);
        }

        public void Dispose()
        {
            _activeState?.Exit();
            _activeState = null;
            _currentState = null;
            
            if (_states.Count > 0)
            {
                foreach (var state in _states.Values)
                {
                    _updatingService.UnRegister(state);
                    _fixedUpdatingService.UnRegister(state);
                    state.Dispose();
                }
            }
            
            _logger.Log($"<color=green>State Machine: {GetType().Name} -> Dispose</color>");
        }

        private TState ChangeState<TState>() where TState : State<FSM>, IExitableState
        {
            if (_currentState is not null)
            {
                _updatingService.UnRegister(_currentState);
                _fixedUpdatingService.UnRegister(_currentState);
            }

            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            _currentState = state;
            _logger.Log($"<color=green>State enter: {typeof(TState)}</color>");
            return state;
        }

        private TState GetState<TState>() where TState : State<FSM>, IExitableState
        {
            if(_states.TryGetValue(typeof(TState), out State<FSM> cachedState))
                return (TState)cachedState;

            var state = _stateFactory.CreateState<TState>(this as FSM);
            _states.Add(typeof(TState), state);

            return state;
        }
    }
}