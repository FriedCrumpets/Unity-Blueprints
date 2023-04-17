using UnityEngine;

namespace Blueprints.ServiceLocator
{
    public class ServiceHandler : MonoBehaviour
    {
        private static ServiceLocator _locator;
        
        protected static ServiceLocator Locator 
            => _locator ??= new();

        public static void Get<T>() where T : IService
            => _locator.Get<T>();
    }
}