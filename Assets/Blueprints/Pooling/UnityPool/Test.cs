using Logging;
using UnityEngine;

namespace Pooling
{
    public class Test : MonoBehaviour
    {
        private EnemyPool _pool;
        private bool _logReceived;
         
        private Debugger testLog = new Debugger("Test", Debug.unityLogger.logHandler);
        // private GameLogger testlog2 = new GameLogger("Test", Debug.unityLogger.logHandler);
        // private GameLogger secondLog = new GameLogger("SecondTest", Debug.unityLogger.logHandler);
        
        private void Start()
        {
            _pool = new EnemyPool(10, 10);
            // testLog.Log("start");
            // secondLog.Log("second");
            Application.logMessageReceived += (condition, trace, type) => // type == LogType
            {
                if (!_logReceived)
                {
                    testLog.Log($"Condition: {condition} // Trace: {trace} // Type: {type.ToString()}", this);    
                }
                
                _logReceived = true;
            };
            
            testLog.Log("poo", this);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Spawn"))
            {
                _pool.Spawn();
                // testLog.Log("Spawned");
            }
        }
    }
}