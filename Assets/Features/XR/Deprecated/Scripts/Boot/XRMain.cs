using System;
using Blueprints.Boot;
using UnityEngine;

namespace Features.XR
{
    public class XRMain :  MonoBehaviour
    {
        [SerializeField] private Main main;

        private void Start()
            => main.Boot(gameObject);

        private void OnDestroy()
            => main.Dispose();
    }
}
