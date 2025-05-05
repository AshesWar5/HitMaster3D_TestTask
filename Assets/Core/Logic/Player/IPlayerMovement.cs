using System;

namespace Core.Logic.Player
{
    public interface IPlayerMovement : IMove
    {
        event Action EndTargetMovement;
    }
}