using System;
using MetaSpaces.Plaza.Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;
using Utils;

namespace Features.XR
{
    public class VignetteController : MonoBehaviour
    {
        private Vignette _vignette;
        private VignetteSettings_SO _vignetteSettings;
        private LocomotionProvider _activeTurnProvider;
        private float _fadeDuration;
        
        public Volume Volume { get; set; }

        private VignetteSettings_SO VignetteSettings
        {
            get
            {
                _vignetteSettings =
                    Utilities.LoadResourceIfNull(_vignetteSettings, "Vignette/XRVignetteSettings");

                return _vignetteSettings;
            } 
        }

        public Vignette Vignette
        {
            get
            {
                if (_vignette == null)
                {
                    SetVignette(VignetteSettings.VignetteVolumeProfile.Value);
                }

                return _vignette;
            }
        }

        public void Enable(XRPlayer player)
        {
            SubscribeProviderChanged(player.MovementProvider);
            SubscribeSettings(VignetteSettings);
            SetVignetteTurnProvider(player.MovementProvider.ActiveTurnProvider);

            VignetteSettings.Load();
        }

        public void Disable(XRPlayer player)
        {
            UnSubscribeProviderChanged(player.MovementProvider);
            UnSubscribeSettings(VignetteSettings);
            SetVignetteTurnProvider(null);
        }

        public void SetVignetteTurnProvider(LocomotionProvider provider)
        {
            UnSubscribeProvider(_activeTurnProvider);
            _activeTurnProvider = provider;
            SubscribeProvider(_activeTurnProvider);
        }

        private void SubscribeSettings(VignetteSettings_SO vignetteSettings)
        {
            UnSubscribeSettings(vignetteSettings);
            
            if(vignetteSettings == null)
            {
                return;
            }
            
            vignetteSettings.VignetteVolumeProfile.OnValueChanged += SetVignette;
            vignetteSettings.VignetteActive.OnValueChanged += OnActiveChanged;
            vignetteSettings.VignetteIntensity.OnValueChanged += OnIntensityChanged;
        }

        private void UnSubscribeSettings(VignetteSettings_SO vignetteSettings)
        {
            if(vignetteSettings == null)
            {
                return;
            }
            
            vignetteSettings.VignetteVolumeProfile.OnValueChanged -= SetVignette;
            vignetteSettings.VignetteActive.OnValueChanged -= OnActiveChanged;
            vignetteSettings.VignetteIntensity.OnValueChanged -= OnIntensityChanged;
        }

        private void SubscribeProvider(LocomotionProvider provider)
        {
            UnSubscribeProvider(provider);
            if (provider == null)
            {
                return;
            }

            provider.beginLocomotion += FadeIn;
            provider.endLocomotion += FadeOut;
        }

        private void UnSubscribeProvider(LocomotionProvider provider)
        {
            if (provider == null)
            {
                return;
            }

            provider.beginLocomotion -= FadeIn;
            provider.endLocomotion -= FadeOut;
        }

        private void SubscribeProviderChanged(XRMovementProvider provider)
        {
            UnSubscribeProviderChanged(provider);
            provider.TurnProviderChanged += SetVignetteTurnProvider; 
        }

        private void UnSubscribeProviderChanged(XRMovementProvider provider)
        {
            provider.TurnProviderChanged -= SetVignetteTurnProvider; 
        }
        
        public void FadeIn(LocomotionSystem locomotionSystem)
        {
            StopAllCoroutines();
            StartCoroutine(Fader.Fade(Vignette.intensity.value, VignetteSettings.VignetteIntensity.Value, VignetteSettings.VignetteFadeDuration.Value));
        }

        public void FadeOut(LocomotionSystem locomotionSystem)
        {
            StopAllCoroutines();
            StartCoroutine(Fader.Fade(Vignette.intensity.value, 0, VignetteSettings.VignetteFadeDuration.Value));
        }
        
        public void SetVignette(VolumeProfile volumeProfile)
        {
            if (!volumeProfile.TryGet(out Vignette vignette))
            {
                throw new XRVignetteException($"No Vignette found in provided PostProcessing Volume");
            }

            Volume.profile = volumeProfile;
            _vignette = vignette;
            _vignette.active = VignetteSettings.VignetteActive.Value;
            _vignette.intensity.value = VignetteSettings.VignetteIntensity.Value;
        }
        
        private void OnActiveChanged(bool change)
            => Vignette.active = change;
        
        private void OnIntensityChanged(float change) 
            => Vignette.intensity.value = change;

        private class XRVignetteException : Exception
        {
            public XRVignetteException(string message) : base(message) { }
        }
    }
}