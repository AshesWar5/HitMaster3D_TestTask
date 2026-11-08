using UnityEngine;

namespace Core.Services.Input
{
    public sealed class MobileInputService : InputService
    {
        protected override bool TryGetPointerDownInternal(out Vector2 screenPoint, out int pointerId)
        {
            screenPoint = Vector2.zero;
            pointerId = 0;

            for (int i = 0; i < UnityEngine.Input.touchCount; i++)
            {
                Touch touch = UnityEngine.Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    screenPoint = touch.position;
                    pointerId = touch.fingerId;

                    return true;
                }
            }

            return false;
        }
    }
}
