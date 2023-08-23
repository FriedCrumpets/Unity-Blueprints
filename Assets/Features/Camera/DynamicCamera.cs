namespace Features.Camera
{
    using UnityEngine;
using UnityEngine.InputSystem;

namespace Cameras
{
    [ExecuteAlways]
    public abstract class CameraRig : MonoBehaviour 
    {
        [HideInInspector] public Transform _transform;
        private Vector3 _followVelocity = new Vector3(0f,0f,-5f);
        private Vector3 _rotationVector = Vector3.zero;
        private Transform _rotateAroundTransform;
        private Vector3 _followPosition = Vector3.zero;

        [Header("Follow Settings")]
        public Transform follow;
        public Vector3 followOffset = Vector3.zero;

        public Transform rotateAround;
        public float rotationDampening;
        public Transform lookAt;
        public LayerMask ignoreLayer;
        public float rotationRadius;
        public float followTime;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // RotateAround();
            // Follow();
            // LookAt();
        }

        public abstract void OnScroll(InputAction.CallbackContext context);

        public abstract void OnActivated();

        public abstract void OnLook(InputAction.CallbackContext context);

        /// <summary>
        /// Clamps the angle between -360 and 360 to prevent over rotation
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            angle += angle < -360f ? 360f : angle > 360f ? -360f : 0f;
            return Mathf.Clamp(angle, min, max);
        }

        public void LookAt()
        {
            if (lookAt == null) return;
            _transform.LookAt(lookAt.position);
        }

        public void Follow()
        {
            if(follow == null) return;
            _followPosition = follow.position;

            _followPosition += _rotationVector;
            _followPosition += followOffset;
            _followPosition.x = Mathf.Clamp(_followPosition.x, -10f, 10f);

            _transform.localPosition = Vector3.SmoothDamp(_transform.localPosition, _followPosition, ref _followVelocity, followTime);
        }

        // public void RotateAround()
        public void RotateAround(Vector2 input)
        {
            if(rotateAround == null) return;
            // _rotateAroundTransform.position = rotateAround.position;
            // _rotateAroundTransform.rotation = new Quaternion(input.x, input.y, 0f, 0f);

            var rotationTarget = rotateAround.eulerAngles;
            // var rotationTarget = _rotateAroundTransform.eulerAngles;
            // _rotateVector += new Vector3(point.x, point.y, 0f);
            
            var s = rotationTarget.x * Mathf.PI / 180;
            var t = rotationTarget.y * Mathf.PI / 180;
            // var s = Mathf.Atan(_rotateVector.x/_rotateVector.y);
            // var t = Mathf.Acos(0f/rotationRadius);
            
            var x = -rotationRadius * Mathf.Cos(s) * Mathf.Sin(t);
            var y = -rotationRadius * Mathf.Sin(s) * Mathf.Sin(t);
            var z = -rotationRadius * Mathf.Cos(t); 
            
            _rotationVector = new Vector3(x, y, z);
            // Mathf.Clamp(_rotationVector.x, -80f, 80f);
        }
    }
}
}