using System;
using UnityEngine;

namespace Blueprints.Boot
{
    /// <summary>
    /// Boot up for all manager classes this needs to be put on an object in the scene and will create the singleton instance of anything added 
    /// </summary>
    public class Boot : MonoBehaviour
    {
        [SerializeField] private Main main;

        private void Awake()
            => main = new Main(gameObject);

        private void Start()
            => main.Boot();

        private void OnDestroy()
            => main.Dispose();
    }
}