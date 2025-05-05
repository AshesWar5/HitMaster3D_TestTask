using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Camera
{
    public sealed class CameraFollow : Component
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _smoothSpeed = 0.125f;

        public void SetTarget(Transform target) => _target = target;

        public override void Updating()
        {
            if(_target == null)
                return;
            
            Vector3 desiredPosition = _target.position + _offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(_target);
        }
    }
}