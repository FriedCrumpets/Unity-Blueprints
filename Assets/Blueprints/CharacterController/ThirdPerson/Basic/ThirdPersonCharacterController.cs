using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Blueprints.CharacterController.ThirdPerson
{
    public class ThirdPersonCharacterController : MonoBehaviour, ThirdPersonActions.IMovementActions
    {
        public bool grounded;
        
        [SerializeField] private float speed = 4;
        [SerializeField] private float acceleration = 7;
        [SerializeField] private float maxForce = 1;
        [SerializeField] private float jumpForce = 3;
        [SerializeField] private float xSensitivity = 10f;
        [SerializeField] private float maxLookVaLue = 50;
        [SerializeField] private float minLookVaLue = -50;
        [SerializeField] private float ySensitivity = 10f;
        [SerializeField] private float lookSpeed = 25;
        
        private Rigidbody _rigidBody;
        private Vector2 _moveVector;
        private Vector2 _lookVector;
        private float _lookRotation;
        private Camera _camera;
        private ThirdPersonActions _actions;

        private void Awake()
        {
            var collider = gameObject.AddComponent<CapsuleCollider>();
            collider.center = Vector3.zero;
            collider.radius = .5f;
            collider.height = 2;
            collider.direction = 1; // 1 = up;

            collider.material = Resources.Load<PhysicMaterial>("ThirdPersonCharacterControllerExampleMaterial");
            
            _rigidBody = gameObject.AddComponent<Rigidbody>();
            _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            _camera = Camera.main;
            _actions = new ThirdPersonActions();
            var checkGo = new GameObject("GroundCheck")
            {
                transform =
                {
                    localPosition = Vector3.zero,
                    localRotation = Quaternion.identity,
                    parent = transform
                },
            };
            
            var groundCollider = checkGo.AddComponent<BoxCollider>();
            var colliderPosition = transform.position;
            colliderPosition.y -= transform.position.y;
            groundCollider.center = colliderPosition;
            groundCollider.size = new Vector3(.9f, .1f, .9f);
            groundCollider.isTrigger = true;

            checkGo.AddComponent<GroundCheck>().Init(this);
        }

        private void OnEnable()
            => _actions.Movement.AddCallbacks(this);

        private void Start()
        {
            _actions.Enable();
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void LateUpdate()
        {
            Look();
        }

        private void OnDisable()
            => _actions.Movement.RemoveCallbacks(this);

        private void Move()
        {
            // set velocities
            var currentVelocity = _rigidBody.velocity;
            var targetVelocity = new Vector3(_moveVector.x, 0, _moveVector.y);
            targetVelocity *= speed;

            // align direction
            targetVelocity = transform.TransformDirection(targetVelocity);

            // calculate forces
            var velocityChange = targetVelocity - currentVelocity;
            velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z) * (Time.deltaTime * acceleration); 
            // note: adding { * Time.deltaTime } at the end of the above line adds acceleration over time effect, if acceleration variable is defined 
            // note: could possibly have separate variables for speeding up and slowing down to vary animations

            // limit force on controller
            velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);
            _rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
        }

        private void Look()
        {
            // Turn
            transform.Rotate(Vector3.up * (_lookVector.x * xSensitivity * Time.deltaTime));
            
            // Look
            _lookRotation += -_lookVector.y * ySensitivity * Time.deltaTime;
            _lookRotation = Mathf.Clamp(_lookRotation, minLookVaLue, maxLookVaLue);
            var cameraTransform = _camera.transform;
            var eulerAngles = cameraTransform.eulerAngles;
            eulerAngles = new Vector3(_lookRotation, eulerAngles.y, eulerAngles.z);
            cameraTransform.eulerAngles = eulerAngles;
        }

        private void Jump()
        {
            if(grounded)
                _rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        }

        void ThirdPersonActions.IMovementActions.OnMove(InputAction.CallbackContext context)
            => _moveVector = context.ReadValue<Vector2>();

        void ThirdPersonActions.IMovementActions.OnLook(InputAction.CallbackContext context)
            => _lookVector = context.ReadValue<Vector2>();

        void ThirdPersonActions.IMovementActions.OnJump(InputAction.CallbackContext context)
            => Jump();
    }
}