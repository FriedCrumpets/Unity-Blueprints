using System;
using Features.XR;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Utils;

namespace MetaSpaces.Plaza.Player
{
    [Serializable]
    public class XRMovementProvider
    {
        public event Action<LocomotionProvider> TurnProviderChanged;

        private GameObject _xrBaseObject;
        private MovementSettings_SO _movementSettings;
        
        [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
        [SerializeField] private ActionBasedContinuousTurnProvider continuousTurnProvider;
        [SerializeField] private TeleportationProvider teleportationProvider;
        [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;

        public ActionBasedSnapTurnProvider SnapTurnProvider 
        { 
            get 
            {
                if (snapTurnProvider != null)
                {
                    return snapTurnProvider;
                }

                snapTurnProvider = _xrBaseObject.ForceComponent<ActionBasedSnapTurnProvider>();

                return snapTurnProvider;
            } 
        }
        
        public ActionBasedContinuousTurnProvider ContinuousTurnProvider 
        { 
            get 
            {
                if (continuousTurnProvider != null)
                {
                    return continuousTurnProvider;
                }

                continuousTurnProvider = _xrBaseObject.ForceComponent<ActionBasedContinuousTurnProvider>();

                return continuousTurnProvider;
            } 
        }
        
        public TeleportationProvider TeleportationProvider 
        {
            get 
            {
                if (teleportationProvider != null)
                {
                    return teleportationProvider;
                }

                teleportationProvider = _xrBaseObject.ForceComponent<TeleportationProvider>();

                return teleportationProvider;
            } 
       }
        
        public ActionBasedContinuousMoveProvider ContinuousMoveProvider 
        { 
            get 
            {
                if (continuousMoveProvider != null)
                {
                    return continuousMoveProvider;
                }

                continuousMoveProvider = _xrBaseObject.ForceComponent<ActionBasedContinuousMoveProvider>();

                return continuousMoveProvider;
            }
        }

        public LocomotionProvider ActiveTurnProvider =>
            SnapTurnProvider.enabled ? SnapTurnProvider : ContinuousMoveProvider;

        public void Enable(XRPlayerDeprecated playerDeprecated)
        {
            if (_movementSettings == null)
            {
                return;
            }
            
            UnSubscribe(playerDeprecated.MovementSettings);
            Subscribe(playerDeprecated.MovementSettings);
            playerDeprecated.MovementSettings.Load();
            OnTurnProviderChanged(playerDeprecated.MovementSettings.TurnProvider.Get());
        }
        
        public void Initialise(XRPlayerDeprecated playerDeprecated)
        {
            _xrBaseObject = playerDeprecated.Rig.originBase.gameObject;
            _movementSettings = playerDeprecated.MovementSettings;

            snapTurnProvider = SnapTurnProvider;
            continuousTurnProvider = ContinuousTurnProvider;
            continuousMoveProvider = ContinuousMoveProvider;
            
            // TODO: Re-enable when Teleportation Functionality is working
            teleportationProvider = TeleportationProvider;
            teleportationProvider.enabled = false;

            Enable(playerDeprecated);
        }

        public void Disable(XRPlayerDeprecated playerDeprecated)
        {
            UnSubscribe(playerDeprecated.MovementSettings);
        }

        private void Subscribe(MovementSettings_SO movementSettings)
        {
            movementSettings.TurnProvider.Notifier += OnTurnProviderChanged;
            movementSettings.ContinuousTurnSpeed.Get("value").Notifier += OnContinuousTurnSpeedChanged;
            movementSettings.MovementSpeed.Get("value").Notifier += OnMovementSpeedChanged;
            movementSettings.SnapTurnDegrees.Get("value").Notifier += OnSnapTurnDegreesChanged;
        }
        
        private void UnSubscribe(MovementSettings_SO movementSettings)
        {
            movementSettings.TurnProvider.Notifier -= OnTurnProviderChanged;
            movementSettings.ContinuousTurnSpeed.Get("value").Notifier -= OnContinuousTurnSpeedChanged;
            movementSettings.MovementSpeed.Get("value").Notifier -= OnMovementSpeedChanged;
            movementSettings.SnapTurnDegrees.Get("value").Notifier -= OnSnapTurnDegreesChanged;
        }

        private void OnTurnProviderChanged(bool change)
        {
            Action action = change ? EnableSnapTurn : EnableContinuousTurn;
            action.Invoke();
            TurnProviderChanged?.Invoke(change ? SnapTurnProvider : ContinuousTurnProvider);
        }
        
        private void OnContinuousTurnSpeedChanged(float change) 
            => ContinuousTurnProvider.turnSpeed = change;

        private void OnMovementSpeedChanged(float change) 
            => ContinuousMoveProvider.moveSpeed = change;

        private void OnSnapTurnDegreesChanged(float change) 
            => SnapTurnProvider.turnAmount = change;

        private void EnableSnapTurn() 
            => ToggleTurnLocomotion(true);

        private void EnableContinuousTurn() 
            => ToggleTurnLocomotion(false);

        private void ToggleTurnLocomotion(bool toggle)
        {
            SnapTurnProvider.enabled = toggle;
            ContinuousTurnProvider.enabled = !toggle;
        }
    }
}