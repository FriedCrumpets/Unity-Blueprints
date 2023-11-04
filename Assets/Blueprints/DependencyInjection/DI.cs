namespace Blueprints.DependencyInjection
{
    public class DI {
        private static readonly DInjection Injection;

        static DI() {
            Injection = new DInjection();
        }
        
        public void AddSingleton<T>(T @object) {
            Injection.AddSingleton(@object);
        }

        public void AddTransient<TType, TConcrete>() {
            Injection.AddTransient<TType, TConcrete>();
        }

        public static T Resolve<T>() {
            return Injection.Resolve<T>();
        }
    }
}