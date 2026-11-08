using System;
using Core.Infrastructures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.UI
{
    public class ButtonView : ViewUI
    {
        public event Action Click;

        [SerializeField] private Button _button;
        
        private TextView _text;
        private ImageView _imageButton;
        private bool _isInteractive = true;

        public string Text => _text.Text;

        protected override void OnInitialize()
        {
            _text = _button.GetComponentInChildren<TextView>();
            _imageButton = _button.GetComponent<ImageView>();
            
            _button.onClick.AddListener(OnClick);
            
            OnInitializeInternal();
        }

        protected override void OnDispose()
        {
            _button.onClick.RemoveListener(OnClick);
            
            OnDisposeInternal();
        }

        public void UpdateInteractive(bool interactive) =>
            _isInteractive = interactive;

        public void SetText(string text) =>
            _text.SetText(text);
        
        public void SetTextColor(Color color) =>
            _text.SetColor(color);

        public void SetColor(Color color) =>
            _imageButton.SetColor(color);

        public void SetSprite(Sprite sprite) =>
            _imageButton.SetSprite(sprite);

        private void OnClick()
        {
            if(!_isInteractive)
                return;
            
            Click?.Invoke();
        }

        protected virtual void OnInitializeInternal() { }
        
        protected virtual void OnDisposeInternal() { }
    }
}