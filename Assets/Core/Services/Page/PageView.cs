using System;
using Core.Infrastructures;
using UnityEngine;

namespace Core.Services.Page
{
    public class PageView : ViewUI
    {
        public event Action PageEnter;
        public event Action PageExit;
        public bool IsInitialized { get; private set; }
        public bool IsOpenPage { get; private set; }
        
        [SerializeField] private PageType _type;
        [SerializeField] private bool _isFree;
        
        public PageType Type => _type;
        public bool IsFree => _isFree;

        protected override void OnInitialize()
        {
            OnInitializeInternal();
            IsInitialized = true;
        }

        protected override void OnDispose()
        {
            OnDisposeInternal();
            IsInitialized = false;
        }

        protected override void OnEnter()
        {
            OnEnterInternal();
            IsOpenPage = true;
            PageEnter?.Invoke();
        }

        protected override void OnExit()
        {
            OnExitInternal();
            IsOpenPage = false;
            PageExit?.Invoke();
        }

        protected virtual void OnInitializeInternal() { }
        
        protected virtual void OnDisposeInternal() { }
        
        protected virtual void OnEnterInternal() { }
        
        protected virtual void OnExitInternal() { }
    }
}