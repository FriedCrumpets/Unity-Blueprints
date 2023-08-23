using System;
using Blueprints.DoD.v1;
using Blueprints.Facade;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.XR
{
    [CreateAssetMenu(fileName = "VignetteSettings", menuName = "ScriptableObjects/XR/VignetteSettings", order = 0)]
    public class VignetteSettings_SO : ScriptableObject
    {
        [field: SerializeField] public IData<bool> VignetteActive { get; private set; }
            = new Data<bool>(true);
        
        [field: SerializeField] public IDataSet<float> VignetteIntensity { get; private set; }
            = new ClampedDataSet(.5f, 0, 1);
        
        [field: SerializeField] public IDataSet<float> VignetteFadeDuration { get; private set; }
            = new ClampedDataSet(.5f, .25f, .75f);
        
        [field: SerializeField] public IData<VolumeProfile> VignetteVolumeProfile { get; private set; }
    }

    public static class VignetteSettingsExtensions
    {
        [ContextMenu("Load")]
        public static void Load(this VignetteSettings_SO settings)
        {
            if (settings.VignetteVolumeProfile == null)
            { 
                settings.VignetteVolumeProfile.Set(Resources.Load<VolumeProfile>("Vignette/XRVignetteProfile"));  
            } 
            
            settings.VignetteActive.Set(PlayerPrefs.GetInt(nameof(settings.VignetteActive), 1) == 1); 
            settings.VignetteIntensity.Set("value", PlayerPrefs.GetFloat(nameof(settings.VignetteIntensity), .5f));
            settings.VignetteFadeDuration.Set("value", PlayerPrefs.GetFloat(nameof(settings.VignetteFadeDuration), .1f));
        }
        
        [ContextMenu("Save")]
        public static void Save(this VignetteSettings_SO settings)
        {
            PlayerPrefs.SetInt(nameof(settings.VignetteActive), settings.VignetteActive.Get() ? 1 : 0);
            PlayerPrefs.SetFloat(nameof(settings.VignetteIntensity), settings.VignetteIntensity.Read("value"));
            PlayerPrefs.SetFloat(nameof(settings.VignetteFadeDuration), settings.VignetteFadeDuration.Read("value"));
        }
    }
}