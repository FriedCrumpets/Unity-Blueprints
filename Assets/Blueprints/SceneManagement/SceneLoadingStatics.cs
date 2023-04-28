using Blueprints.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blueprints.SceneManagement
{
    public static class SceneLoadingStatics
    {
        public static void LoadLevelAsync(this MonoBehaviour behaviour, string sceneName, LoadSceneMode mode)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            
            Debug.Log($"{behaviour.name} Starting Scene Load [scene:{sceneName}] async...");

            // Prevents loading the scene immediately
            operation.allowSceneActivation = false;
            
            behaviour.ChainCoroutine(
                CoroutineUtils.WaitUntil(() => operation.progress > 0.9f),
                CoroutineUtils.Do(() =>
                {
                    Debug.Log($"{behaviour.name} Loaded Scene [scene:{sceneName}]");
                    operation.allowSceneActivation = true;
                })
            );
        }

        public static void UnloadLevel(this MonoBehaviour behaviour, string sceneName)
        {
            var operation = SceneManager.UnloadSceneAsync(sceneName);

            behaviour.ChainCoroutine(
                CoroutineUtils.WaitUntil(() => operation.isDone),
                // we need to manually unload the scenes assets so... we do this
                CoroutineUtils.Do(() =>
                {
                    Debug.Log($"{behaviour.name} Unloaded Scene [scene:{sceneName}]");
                    Resources.UnloadUnusedAssets();
                })
            );
        }
    }
}