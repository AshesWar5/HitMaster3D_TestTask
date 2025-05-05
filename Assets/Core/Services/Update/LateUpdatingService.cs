using Core.Infrastructures;
using Zenject;

namespace Core.Services.Update
{
    public sealed class LateUpdatingService : UpdateService<ILateUpdating>, ILateUpdatingService, ILateTickable
    {
        public void LateTick()
        {
            if(!_isInitialized)
                return;
            
            if(_updatesAll.Count == 0)
                return;

            for (int i = 0; i < _updatesAll.Count; i++)
            {
                _updatesAll[i].LateUpdating();
            }
        }
    }
}