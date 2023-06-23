using Blueprints.Boot;
using UnityEngine;

namespace Features.XR
{
    public class XRMain :  MonoBehaviour
    {
        [SerializeField] private Main main;

        private void Awake()
            => main = new(gameObject);

        private void Start()
            => main.Boot();

        private void OnDestroy()
            => main.Dispose();
    }
}
