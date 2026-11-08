using UnityEngine;

namespace Core.Services.Input
{
    public sealed class StandaloneInputService : InputService
    {
        private const int MOUSE_POINTER_ID = -1;

        protected override bool TryGetPointerDownInternal(out Vector2 screenPoint, out int pointerId)
        {
            screenPoint = UnityEngine.Input.mousePosition;
            pointerId = MOUSE_POINTER_ID;

            return UnityEngine.Input.GetMouseButtonDown(0);
        }
    }
}
