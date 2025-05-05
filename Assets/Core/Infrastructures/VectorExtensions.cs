using UnityEngine;

namespace Core.Infrastructures
{
    public static class VectorExtensions
    {
        public static bool DistanceOptimize(this Vector3 a, Vector3 b, float distance) =>
            (a - b).sqrMagnitude <= distance * distance;
    }
}