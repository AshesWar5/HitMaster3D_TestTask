using UnityEngine;
using Component = Core.Infrastructures.Component;

namespace Core.Logic.Camera
{
    public sealed class CameraScaler : Component
    {
        [SerializeField] private float _targetWidth = 15f;

        protected override void OnInitialize()
        {
            UnityEngine.Camera cam = GetComponent<UnityEngine.Camera>();

            float targetAspect = _targetWidth / cam.orthographicSize / 2f;
            float screenAspect = Screen.width / (float)Screen.height;
            
            if (screenAspect >= targetAspect)
            {
                cam.orthographicSize = _targetWidth / (2f * screenAspect);
            }
            else
            {
                cam.orthographicSize = _targetWidth / 2f;
            }
        }
    }
}