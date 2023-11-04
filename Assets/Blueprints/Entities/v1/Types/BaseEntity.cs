using System.Collections.Generic;
using Blueprints.Messenger;
using UnityEngine;

namespace Blueprints.Entities
{
    public class BaseEntity<T> : MonoBehaviour where T : SOEntity
    {
        [field: SerializeField] public T Data { get; private set; }
        
        public Courier Courier { get; private set; }

        private IList<ISystem<BaseEntity<T>>> Systems { get; set; }
        
        private void Awake()
        {
            Systems = GetComponents<ISystem<BaseEntity<T>>>();
            Courier = new Courier();
        }
        
        private void Start()
        {
            foreach (var system in Systems)
                system.Init(this);
        }
        
        private void OnEnable()
        {
            foreach(IActivatable activatable in Systems)
                activatable.Activate();
        }

        private void OnDisable()
        {
            foreach(IActivatable activatable in Systems)
                activatable.Deactivate();
        }

        private void OnDestroy()
        {
            foreach (var system in Systems)
                system.Deinit();
        }
    }
}