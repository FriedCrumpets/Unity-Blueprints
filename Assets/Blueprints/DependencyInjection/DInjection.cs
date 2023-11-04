using System;
using System.Collections.Generic;

namespace Blueprints.DependencyInjection {
    public class DInjection {
        private Dictionary<Type, Dependency> Dependencies { get; } = new();
        
        public void AddSingleton<T>(T @object) {
            Dependencies.TryAdd(typeof(T), new Dependency(DependencyType.Singleton, @object));
        }

        public void AddTransient<TType, TConcrete>() {
            Dependencies.TryAdd(typeof(TType), new Dependency(DependencyType.Transient, typeof(TConcrete)));
        }
        
        public T Resolve<T>() {
            if (Dependencies.TryGetValue(typeof(T), out var dependency))
                return dependency.Type switch {
                    DependencyType.Singleton => (T)dependency.Object,
                    DependencyType.Transient => (T)Activator.CreateInstance(dependency.Object.GetType()),
                };

            throw new ArgumentException("Requested Dependency Does not exist");
        }
    }
}