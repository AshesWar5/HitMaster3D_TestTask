using System;
using System.Collections.Generic;
using Core.Infrastructures.Logger;
using UnityEngine;
using Zenject;

namespace Core.Infrastructures
{
    public class Component : MonoBehaviour, IInitialize, IDisposable, IEnter, IExit, IShow, IHide, IUpdating,
        IFixedUpdating, ILateUpdating
    {
        [SerializeField] private bool _isInitializeAllChildren = true;
        [SerializeField] private bool _isAutoUpdateThisComponent;

        private List<Component> _components;
        protected ILoggerService _logger;
        private bool _isInitialized;

        public bool IsActiveComponent => gameObject.activeSelf;

        [Inject]
        public void Construct(ILoggerService logger) => _logger = logger;

        public void Initialize()
        {
            if(_isInitialized)
                return;
            
            if (_isInitializeAllChildren)
            {
                _components = new List<Component>(transform.childCount);
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).TryGetComponent(out Component component);
                    if (component is not null)
                    {
                        component.Initialize();
                        _components.Add(component);
                    }
                }
            }

            OnInitialize();

            _isInitialized = true;
        }

        public void Dispose()
        {
            if(!_isInitialized)
                return;
            
            if (_isInitializeAllChildren)
            {
                foreach (Component component in _components)
                {
                    component.Dispose();
                }
            }

            OnDispose();

            _isInitialized = false;
        }

        public void Enter()
        {
            if(!_isInitialized)
                return;

            if (_isInitializeAllChildren)
            {
                foreach (Component component in _components)
                {
                    component.Enter();
                }
            }

            OnEnter();
        }

        public void Exit()
        {
            if(!_isInitialized)
                return;

            if (_isInitializeAllChildren)
            {
                foreach (Component component in _components)
                {
                    component.Exit();
                }
            }

            OnExit();
        }

        public void Show()
        {
            if(this == null)
                return;

            gameObject.SetActive(true);
            OnShow();
        }

        public void Hide()
        {
            if(this == null)
                return;

            gameObject.SetActive(false);
            OnHide();
        }

        private void Update()
        {
            if(!_isAutoUpdateThisComponent)
                return;
            
            Updating();
        }
        
        private void FixedUpdate()
        {
            if(!_isAutoUpdateThisComponent)
                return;
            
            FixedUpdating();
        }

        private void LateUpdate()
        {
            if(!_isAutoUpdateThisComponent)
                return;
            
            LateUpdating();
        }

        protected virtual void OnInitialize() { }
        
        protected virtual void OnDispose() { }
        
        protected virtual void OnShow() { }
        
        protected virtual void OnHide() { }
        
        protected virtual void OnEnter() { }
        
        protected virtual void OnExit() { }
        
        public virtual void Updating() { }
        
        public virtual void FixedUpdating() { }
        
        public virtual void LateUpdating() { }
    }
}