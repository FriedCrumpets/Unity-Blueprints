using System.Collections;
using Blueprints.Components;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.Camera.Deprecated
{
    public class OrbitCameraRig : MonoComponent, ICameraRig
    {
        private Vector3 _rotationVector;
        public Transform FollowTarget { get; private set; }
        public Transform LookAtTarget { get; private set; }
        public Transform GhostCamera { get; private set; }

        public void Init(Transform target, Transform lookAt)
        {
            if (GhostCamera == null)
            {
                var ghostGo = new GameObject("Ghost")
                {
                    transform =
                    {
                        parent = transform
                    }
                };
                
                GhostCamera = ghostGo.transform;
            }

            FollowTarget = target;
            LookAtTarget = lookAt;
        }
        
        public void Activate(DynamicCamera camera)
        {
            if (FollowTarget == null)
            {
                Debug.LogError($"{nameof(OrbitCameraRig)}: Cannot activate a {nameof(ICameraRig)} that has not " +
                               "yet been initialized");
                return;
            }
            
            StartCoroutine(Follow(transform, FollowTarget));
            StartCoroutine(LookAt(GhostCamera, LookAtTarget));
        }
        
        public void Deactivate()
        {
            StopAllCoroutines();
        }
        
        public void DeInit()
        {
            Deactivate();
            Destroy(GhostCamera.gameObject);
        }

        private IEnumerator Follow(Transform transform, Transform target)
        {
            while (true)
            {
                var velocity = Vector3.zero;
                transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, followTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, followDegrees);
                yield return null;
            }
        }

        private IEnumerator LookAt(Transform transform, Transform target)
        {
            while (true)
            {
                transform.LookAt(target.position);
                yield return null;
            }
        }

        private IEnumerator Orbit(Vector2 orbitAngle)
        {
            while (true)
            {
                // _rotateAroundTransform.position = rotateAround.position;
                // _rotateAroundTransform.rotation = new Quaternion(input.x, input.y, 0f, 0f);

                var rotationTarget = transform.eulerAngles;
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
                yield return null;
            }
        }

        public void OnScroll(InputAction.CallbackContext context)
        {
            
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }
        
        [HideInInspector] public Transform _transform;
        private Transform _rotateAroundTransform;

        public Vector3 followOffset = Vector3.zero;

        public float rotationRadius;
        public float followTime;
        public float followDegrees;

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
    }
}