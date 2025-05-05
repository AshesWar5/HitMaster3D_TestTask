namespace Core.Infrastructures.GameFlow
{
    public abstract class GameFlowManager : IGameFlow
    {
        protected bool _isStartedGame { get; private set; }
        
        public void StartGame()
        {
            if(_isStartedGame)
                return;
            
            OnStartGameInternal();

            _isStartedGame = true;
        }

        public void StopGame()
        {
            if(!_isStartedGame)
                return;
            
            OnStopGameInternal();

            _isStartedGame = false;
        }

        public void Updating()
        {
            if(!_isStartedGame)
                return;

            OnUpdatingGameInternal();
        }
        
        public void FixedUpdating()
        {
            if(!_isStartedGame)
                return;

            OnFixedUpdatingGameInternal();
        }

        protected abstract void OnStartGameInternal();
        
        protected abstract void OnStopGameInternal();
        
        protected virtual void OnUpdatingGameInternal() { }
        
        protected virtual void OnFixedUpdatingGameInternal() { }
    }
}