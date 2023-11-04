namespace Blueprints.DependencyInjection {
    internal class Dependency
    {
        public object Object { get; } 
        public DependencyType Type { get; } 
            
        public Dependency(DependencyType type, object @object) {
            Type = type;
            Object = @object;
        }
    }
}