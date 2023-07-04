namespace Blueprints.Utility
{
    public class ServiceHandler
    {
        private static Locator _locator;
        
        protected static Locator Locator 
            => _locator ??= new();

        public static T Get<T>() where T : IService
            => _locator.Get<T>();

        public void Provide<T>(T service) where T : IService
            => _locator.Add(service);
    }
}