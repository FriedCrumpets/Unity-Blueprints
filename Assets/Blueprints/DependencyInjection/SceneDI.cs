using UnityEngine;

namespace Blueprints.DependencyInjection {
    [CreateAssetMenu(fileName = "Scene DI", menuName = "Utils/Scene Dependency Injection", order = 0)]
    public class SceneDI : ScriptableObject {
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