using System;
using System.Collections.Generic;
using Zenject;

namespace Core.Services.Update
{
    public abstract class UpdateService<T> : IInitializable, IDisposable where T : IUpdate
    {
        private readonly List<T> _updates;

        protected bool _isInitialized;
        protected IReadOnlyList<T> _updatesAll => _updates;

        protected UpdateService()
        {
            _updates = new List<T>();
        }

        public void Initialize() =>
            _isInitialized = true;

        public void Dispose()
        {
            _isInitialized = false;
            
            _updates.Clear();
        }

        public void Register(T update)
        {
            if(!_isInitialized)
                return;
            
            if(!_updates.Contains(update))
                _updates.Add(update);
        }
        
        public void UnRegister(T update)
        {
            if(!_isInitialized)
                return;

            if(_updates.Contains(update))
                _updates.Remove(update);
        }
    }
}