using Cysharp.Threading.Tasks;

namespace Core.Infrastructures.GameFlow
{
    public abstract class GameFlowManager : IGameFlow
    {
        protected bool _isStartedGame { get; private set; }
        
        public async UniTask StartGame()
        {
            if(_isStartedGame)
                return;
            
            await OnStartGameInternal();

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

        protected abstract UniTask OnStartGameInternal();
        
        protected abstract void OnStopGameInternal();
        
        protected virtual void OnUpdatingGameInternal() { }
        
        protected virtual void OnFixedUpdatingGameInternal() { }
    }
}