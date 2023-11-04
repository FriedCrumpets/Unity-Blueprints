using UnityEngine;

namespace Blueprints.CharacterController.ThirdPerson
{
    public class GroundCheck : MonoBehaviour
    {
        private ThirdPersonCharacterController _controller;
        
        public void Init(ThirdPersonCharacterController controller)
            => _controller = controller;
        
        private void OnTriggerEnter(Collider other)
        {
            if( other.gameObject != _controller.gameObject )
                _controller.grounded = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if( other.gameObject != _controller.gameObject )
                _controller.grounded = false;
        }
    }
}