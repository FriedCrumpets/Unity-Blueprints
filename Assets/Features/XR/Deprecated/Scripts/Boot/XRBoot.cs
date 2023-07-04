using System;
using System.Collections;
using Blueprints.Core;
using Blueprints.EventBus;
using Logging;
using UnityEngine;
using UnityEngine.XR.Management;

namespace Features.XR
{
    public enum XRBootProgress
    {
        Searching,
        NotLocated,
        Located,
        ShutDown
    }
    
    // todo: this class doesn't work. Fix it.
    // todo: while you're at it reformat the entire XR library to be object oriented, not this current crappy bollocks
    [Serializable]
    public class XRBoot
    {
        [SerializeField] private bool repeatCheck = true;
        [SerializeField] private float repeatXRCheckSeconds = 2.5f;
        
        public EventBus<XRBootProgress> Progress;

        private WaitForSeconds _waitForSeconds;
        private Debugger _log;

        public XRBoot(Debugger logger = null)
        {
            _log = logger;
            _waitForSeconds = new WaitForSeconds(repeatXRCheckSeconds);
        }
        
        public bool XRActive { get; private set; }
        
        // todo : cleanup
        public IEnumerator StartXR()
        {
            _log?.Log("Initializing XR...", this);
            Progress.Publish(XRBootProgress.Searching);

            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null && !XRActive)
            {
                XRActive = false;
                if (repeatCheck)
                {
                    Progress.Publish(XRBootProgress.NotLocated);
                    _log?.Log($"XR Not found... Repeating check in {repeatXRCheckSeconds} seconds", this);
                    yield return _waitForSeconds;
                    yield return StartXR();
                }
                
                _log?.Log($"Initializing XR Failed. Check Editor or Player log for details.", this);
            }
            else
            {
                _log?.Log($"Starting XR...", this);
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                XRActive = true;
                Progress.Publish(XRBootProgress.Located);
            }
        }

        public void StopXR()
        {
            _log?.Log($"Stopping XR...", this);

            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            _log?.Log($"XR Stopped", this);
            XRActive = false;
            Progress.Publish(XRBootProgress.ShutDown);
        }
    }
}