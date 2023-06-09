using Blueprints.Core;
using Logging;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public SessionLog SessionLogger = new();

        private void Start()
        {
            SessionLogger.StartSession();
        }

        private void OnApplicationQuit()
        {
            SessionLogger.EndSession();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Next Scene"))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}