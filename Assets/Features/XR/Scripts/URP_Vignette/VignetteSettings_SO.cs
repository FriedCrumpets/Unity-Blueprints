using System;
using Blueprints.Facade;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.XR
{
    [CreateAssetMenu(fileName = "VignetteSettings", menuName = "ScriptableObjects/XR/VignetteSettings", order = 0)]
    public class VignetteSettings_SO : ScriptableObject
    {
        [field: SerializeField] public Setting<bool> VignetteActive { get; private set; }
            = new(true, true);
        
        [field: SerializeField] public ClampedSetting VignetteIntensity { get; private set; }
            = new(.5f, 0, 1);
        
        [field: SerializeField] public ClampedSetting VignetteFadeDuration { get; private set; }
            = new(.5f, .25f, .75f);
        
        [field: SerializeField] public Setting<VolumeProfile> VignetteVolumeProfile { get; private set; }
    }

    public static class VignetteSettingsExtensions
    {
        [ContextMenu("Load")]
        public static void Load(this VignetteSettings_SO settings)
        {
            if (settings.VignetteVolumeProfile.Value == null)
            { 
                settings.VignetteVolumeProfile.Value = Resources.Load<VolumeProfile>("Vignette/XRVignetteProfile");  
            } 
            
            settings.VignetteActive.Value = PlayerPrefs.GetInt(nameof(settings.VignetteActive), 1) == 1; 
            settings.VignetteIntensity.Value = PlayerPrefs.GetFloat(nameof(settings.VignetteIntensity), .5f);
            settings.VignetteFadeDuration.Value = PlayerPrefs.GetFloat(nameof(settings.VignetteFadeDuration), .1f);
        }
        
        [ContextMenu("Save")]
        public static void Save(this VignetteSettings_SO settings)
        {
            PlayerPrefs.SetInt(nameof(settings.VignetteActive), settings.VignetteActive.Value ? 1 : 0);
            PlayerPrefs.SetFloat(nameof(settings.VignetteIntensity), settings.VignetteIntensity.Value);
            PlayerPrefs.SetFloat(nameof(settings.VignetteFadeDuration), settings.VignetteFadeDuration.Value);
        }
    }
}