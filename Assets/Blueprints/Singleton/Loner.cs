using System;
using Blueprints.Utility;

namespace Blueprints.Core
{
    public abstract class Loner<T> where T : Loner<T>
    {
        private static T Instance;

        protected Loner()
        {
            if (Instance != null)
                return;

            Instance = CreateInstance();
        }

        private static T CreateInstance()
        {
            Instance = Activator.CreateInstance<T>();

            if (Instance is ILoadable loadable)
                loadable.Load();

            return Instance;
        }

        public static void Destroy()
        {
            switch (Instance)
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

            Instance = default;
        }
    }
}