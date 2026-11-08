using Core.Infrastructures;
using Core.Infrastructures.Update;
using Zenject;

namespace Core.Services.Update
{
    public sealed class FixedUpdatingService : UpdateService<IFixedUpdating>, IFixedUpdatingService, IFixedTickable
    {
        public void FixedTick()
        {
            if(!_isInitialized)
                return;
            
            if(_updatesAll.Count == 0)
                return;

            for (int i = 0; i < _updatesAll.Count; i++)
            {
                _updatesAll[i].FixedUpdating();
            }
        }
    }
}