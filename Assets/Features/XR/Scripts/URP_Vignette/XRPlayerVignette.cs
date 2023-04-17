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

        public void Enable(XRPlayer player)
        {
            if (VignetteController == null)
            {
                return;
            }
            
            VignetteController.Enable(player);
            VignetteController.SetVignetteTurnProvider(player.MovementProvider.ActiveTurnProvider);
        }
        
        public void Initialise(XRPlayer player)
        {
            CreateVignetteObject(player.Rig.originBase);
            _movementSettings = player.MovementSettings;
            Enable(player);
        }

        public void Disable(XRPlayer player)
        {
            if(VignetteController == null) { return; }
            
            VignetteController.Disable(player);
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