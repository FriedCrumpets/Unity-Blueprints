using UnityEngine;

namespace Blueprints.DependencyInjection {
    public class MonoDI : MonoBehaviour {
        private DInjection Injection { get; set; }

        private void Awake() {
            Injection = new DInjection();
        }
        
        public void AddSingleton<T>(T @object) {
            Injection.AddSingleton(@object);
        }

        public void AddTransient<TType, TConcrete>() {
            Injection.AddTransient<TType, TConcrete>();
        }

        public T Resolve<T>() {
            return Injection.Resolve<T>();
        }
    }
}