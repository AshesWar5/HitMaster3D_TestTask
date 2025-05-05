using Core.Infrastructures;
using Zenject;

namespace Core.Services.Update
{
    public sealed class UpdatingService : UpdateService<IUpdating>, IUpdatingService, ITickable
    {
        public void Tick()
        {
            if(!_isInitialized)
                return;
            
            if(_updatesAll.Count == 0)
                return;

            for (int i = 0; i < _updatesAll.Count; i++)
            {
                _updatesAll[i].Updating();
            }
        }
    }
}