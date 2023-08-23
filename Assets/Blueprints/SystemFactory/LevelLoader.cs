using System;
using Blueprints.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Blueprints.SystemFactory
{
    public static class LevelLoader
    {
        public static event Action<float> LoadingProgress;
        
        public static void LoadLevelAsync(this MonoBehaviour behaviour, string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, mode);
            
            Debug.Log($"{behaviour.name} Starting Scene Load [scene:{sceneName}] async...");

            // Prevents loading the scene immediately
            operation.allowSceneActivation = false;
            
            behaviour.ChainCoroutine(
                CoroutineUtils.WaitUntil(() =>
                {
                    LoadingProgress?.Invoke(operation.progress);
                    return operation.progress > 0.95f;
                }),
                CoroutineUtils.Do(() =>
                {
                    Debug.Log($"{behaviour.name} Loaded Scene [scene:{sceneName}]");
                    operation.allowSceneActivation = true;
                }),CoroutineUtils.WaitUntil(() =>
                {
                    LoadingProgress?.Invoke(operation.progress);
                    return operation.isDone;
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