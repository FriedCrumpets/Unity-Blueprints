using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

namespace Features.XR
{
    [Serializable]
    public class XRPlayerVignette
    {
        public const string VIGNETTE_OBJECT_NAME = "XRVignette";

        private MovementSettings_SO _movementSettings;

        public VignetteController VignetteController { get; private set; } = null;

        public void Enable(XRPlayerDeprecated playerDeprecated)
        {
            if (VignetteController == null)
            {
                return;
            }
            
            VignetteController.Enable(playerDeprecated);
            VignetteController.SetVignetteTurnProvider(playerDeprecated.MovementProvider.ActiveTurnProvider);
        }
        
        public void Initialise(XRPlayerDeprecated playerDeprecated)
        {
            CreateVignetteObject(playerDeprecated.Rig.originBase);
            _movementSettings = playerDeprecated.MovementSettings;
            Enable(playerDeprecated);
        }

        public void Disable(XRPlayerDeprecated playerDeprecated)
        {
            if(VignetteController == null) { return; }
            
            VignetteController.Disable(playerDeprecated);
            VignetteController.SetVignetteTurnProvider(null);
        }

        private GameObject CreateVignetteObject(Component parent)
        {
            var vignetteObject = new GameObject();
            
            vignetteObject.name = VIGNETTE_OBJECT_NAME;
            vignetteObject.transform.parent = parent.transform;
            var vignetteVolume = vignetteObject.AddComponent<Volume>();
            VignetteController = vignetteObject.AddComponent<VignetteController>();
            VignetteController.Volume = vignetteVolume;

            return vignetteObject;
        }
    }
}