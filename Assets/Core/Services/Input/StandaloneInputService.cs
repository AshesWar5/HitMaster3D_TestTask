using System;
using UnityEngine;
using Zenject;

namespace Core.Services.Input
{
    public sealed class StandaloneInputService : IInputService, ITickable
    {
        public event Action<Vector3> ShootDirection;
        
        public void Tick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Camera.main.transform.forward,
                    Camera.main.transform.position + new Vector3(0, 0, 15));
                Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 v = ray.GetPoint(distance);
                    ShootDirection?.Invoke(v);
                }
            }
        }
    }
}