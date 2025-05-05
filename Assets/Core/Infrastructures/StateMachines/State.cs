using System;

namespace Core.Infrastructures.StateMachines
{
    public abstract class State<FSM> : IInitialize, IDisposable, IUpdating, IFixedUpdating
        where FSM : IStateMachine<FSM>
    {
        protected FSM _stateMachine { get; }
        protected bool _isInitialized { get; private set; }

        protected State(FSM stateMachine) =>
            _stateMachine = stateMachine;

        public void Initialize()
        {
            if (!_isInitialized)
            {
                OnInitializeInternal();

                _isInitialized = true;
            }
        }

        public void Dispose()
        {
            if (_isInitialized)
            {
                OnDisposeInternal();

                _isInitialized = false;
            }
        }

        public void Updating()
        {
            if(_isInitialized)
                OnUpdatingInternal();
        }
        
        public void FixedUpdating()
        {
            if(_isInitialized)
                OnFixedUpdatingInternal();
        }
        
        protected virtual void OnInitializeInternal() { }
        
        protected virtual void OnDisposeInternal() { }
        
        protected virtual void OnUpdatingInternal() { }
        
        protected virtual void OnFixedUpdatingInternal() { }
    }
}