using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core.Services.Input
{
    public abstract class InputService : IInputService, ITickable
    {
        private const float DISTANCE_SHOOT_PLANE = 15;

        public event Action<Vector3> ShootDirection;

        private Camera _camera;

        public void Tick()
        {
            if(!TryGetPointerDownInternal(out Vector2 screenPoint, out int pointerId))
                return;

            if(IsPointerOverUI(pointerId))
                return;

            if (TryGetWorldPoint(screenPoint, out Vector3 worldPoint))
            {
                ShootDirection?.Invoke(worldPoint);
            }
        }

        private bool IsPointerOverUI(int pointerId) =>
            EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);

        private bool TryGetWorldPoint(Vector2 screenPoint, out Vector3 worldPoint)
        {
            worldPoint = Vector3.zero;

            if (_camera == null)
            {
                _camera = Camera.main;
            }

            if(_camera == null)
                return false;

            Transform cameraTransform = _camera.transform;
            var plane = new Plane(cameraTransform.forward,
                cameraTransform.position + cameraTransform.forward * DISTANCE_SHOOT_PLANE);
            Ray ray = _camera.ScreenPointToRay(screenPoint);

            if(!plane.Raycast(ray, out float distance))
                return false;

            worldPoint = ray.GetPoint(distance);

            return true;
        }

        protected abstract bool TryGetPointerDownInternal(out Vector2 screenPoint, out int pointerId);
    }
}
