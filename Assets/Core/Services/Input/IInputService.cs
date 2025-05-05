using System;
using UnityEngine;

namespace Core.Services.Input
{
    public interface IInputService
    {
        event Action<Vector3> ShootDirection;
    }
}