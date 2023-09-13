using System.Numerics;
using UnityEngine.InputSystem;

namespace Blueprints.Entities
{
    public interface IMovementController
    {
        void OnMove(InputActionPhase phase, Vector2 input);
    }
}