using System;
using Core.Infrastructures;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.UI
{
    public class SliderView : ViewUI
    {
        public event Action<float> ValueChanged;
        
        [SerializeField] private Slider _slider;

        private bool _isTriggerValueChange;

        public float Value => _slider.value;
        public float Min => _slider.minValue;
        public float Max => _slider.maxValue;

        protected override void OnInitialize()
        {
            _isTriggerValueChange = true;
            
            _slider.onValueChanged.AddListener(OnValueChange);
            
            InitializeInternal();
        }

        protected override void OnDispose()
        {
            _slider.onValueChanged.RemoveListener(OnValueChange);
            
            DisposeInternal();
        }

        public void SetMin(float value)
        {
            _slider.minValue = value;
            
            OnUpdatedMin(value);
        }

        public void SetMax(float value)
        {
            _slider.maxValue = value;
            
            OnUpdatedMax(value);
        }

        public void SetCurrentValue(float value) =>
            _slider.value = value;

        protected virtual void InitializeInternal() { }

        protected virtual void DisposeInternal() { }

        protected virtual void OnValueChanged(float value) { }

        protected virtual void OnUpdatedMin(float value) { }

        protected virtual void OnUpdatedMax(float value) { }

        private void OnValueChange(float value)
        {
            if (_isTriggerValueChange)
            {
                OnValueChanged(value);

                ValueChanged?.Invoke(value);
            }

            _isTriggerValueChange = true;
        }
    }
}