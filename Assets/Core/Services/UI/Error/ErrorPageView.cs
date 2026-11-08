using System;
using Core.Services.Page;
using UnityEngine;

namespace Core.Services.UI.Error
{
    public sealed class ErrorPageView : PageView
    {
        public event Action RetryClick;

        [SerializeField] private TextView _message;
        [SerializeField] private ButtonView _retry;

        protected override void OnInitializeInternal() =>
            _retry.Click += OnRetryClick;

        protected override void OnDisposeInternal() =>
            _retry.Click -= OnRetryClick;

        public void SetMessage(string message) =>
            _message.SetText(message);

        private void OnRetryClick() =>
            RetryClick?.Invoke();
    }
}
