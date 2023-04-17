using UnityEngine;

namespace Strategy
{
    public class Drone : MonoBehaviour
    {
        private RaycastHit _hit;
        private Vector3 _rayDirection;
        private float _rayAngle = -45f;
        private float _rayDistance = 15f;

        public float speed = 1f;
        public float maxHeight = 5f;
        public float weavingDistance = 1.5f;
        public float fallbackDistance = 20f;

        private void Start()
        {
            _rayDirection = transform.TransformDirection(Vector3.back) * _rayDistance;
            _rayDirection = Quaternion.Euler(_rayAngle, 0, 0) * _rayDirection;
        }

        public void ApplyStrategy(IManeuverable strategy)
        {
            strategy.Maneuver(this);
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, _rayDirection, Color.blue);

            if (Physics.Raycast(transform.position, _rayDirection, out _hit, _rayDistance))
            {
                if (_hit.collider)
                {
                    Debug.DrawRay(transform.position, _rayDirection, Color.green);
                }
            }
        }
    }
}