using UnityEngine;

namespace Blueprints.ServiceLocator
{
    public class ServiceHandler : MonoBehaviour
    {
        private static Locator _locator;
        
        protected static Locator Locator 
            => _locator ??= new();

        public static void Get<T>() where T : IService
            => _locator.Get<T>();
    }
}