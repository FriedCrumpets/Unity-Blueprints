using System.Collections;
using Blueprints.Singleton;
using Blueprints.UnityBus;
using Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Management;

namespace Features.XR
{
    public class XRSystem : Singleton<XRSystem>
    {
        [SerializeField] private bool repeatCheck = true;
        [SerializeField] private float repeatXRCheckSeconds = 2.5f;

        // todo :: use event bus instead here ?? 
        [Space(10)]
        public UnityEvent OnSearchingForXR;
        public UnityEvent OnXRNotFound;
        public UnityEvent OnXRFound;
        public UnityEvent OnXRShutDown;

        private WaitForSeconds _waitForSeconds;
        private Log _log;
        
        public bool XRActive { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CreateInstance();
            
            _log = new Log("XRSystem", Debug.unityLogger.logHandler);
        }

        private IEnumerator Start()
        {
            _waitForSeconds = new WaitForSeconds(repeatXRCheckSeconds);

            yield return StartXR();
        }
        
        // todo : cleanup
        public IEnumerator StartXR()
        {
            _log.Log($"{_log.Name}: Initializing XR...");
            OnSearchingForXR?.Invoke();

            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null && !XRActive)
            {
                XRActive = false;
                if (repeatCheck)
                {
                    OnXRNotFound?.Invoke();
                    _log.Log($"{_log.Name}: XR Not found... Repeating check in {repeatXRCheckSeconds} seconds");
                    yield return _waitForSeconds;
                    yield return StartXR();
                }
                
                _log.Log($"{_log.Name}: Initializing XR Failed. Check Editor or Player log for details.");
            }
            else
            {
                _log.Log($"{_log.Name}: Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                XRActive = true;
                OnXRFound?.Invoke();
            }
        }

        public void StopXR()
        {
            _log.Log($"{_log.Name}: Stopping XR...");

            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            _log.Log($"{_log.Name}: XR Stopped");
            XRActive = false;
            OnXRShutDown?.Invoke();
        }
    }
}