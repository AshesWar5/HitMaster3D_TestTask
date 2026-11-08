using Core.Infrastructures;
using Core.Services.UI;
using UnityEngine;

namespace Core.Logic.Enemy
{
    public class EnemyHealthView : ViewUI
    {
        [SerializeField] private SliderView _health;
        
        public void SetHealthMin(float value) =>
            _health.SetMin(value);
        
        public void SetHealthMax(float value) =>
            _health.SetMax(value);
        
        public void SetCurrentHealth(float value) =>
            _health.SetCurrentValue(value);
    }
}