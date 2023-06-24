using System;
using Blueprints.Utility;

namespace Blueprints.Core
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T _instance;

        public static T Get()
            => _instance;

        private Singleton()
        {
            if (_instance != null)
                return;

            _instance = CreateInstance();
        }

        public static T CreateInstance()
        {
            _instance = Activator.CreateInstance<T>();

            if (_instance is ILoadable loadable)
                loadable.Load();

            return _instance;
        }

        public static void Destroy()
        {
            switch (_instance)
            {
                case null:
                    return;
                case ILoadable loadable:
                    loadable.Save();
                    break;
                case IDisposable disposable:
                    disposable.Dispose();
                    break;
            }

            _instance = default;
        }
    }
}