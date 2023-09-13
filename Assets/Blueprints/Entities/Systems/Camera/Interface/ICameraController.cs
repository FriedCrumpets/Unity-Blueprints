using System.Numerics;
using UnityEngine.InputSystem;

namespace Blueprints.Entities
{
    public interface ICameraController
    {
        void OnLook(InputActionPhase phase, Vector2 input);
        void OnScroll(InputActionPhase phase, float axis);
        void OnChange(InputActionPhase phase);
    }
}