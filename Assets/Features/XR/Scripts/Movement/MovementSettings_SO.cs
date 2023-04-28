using Blueprints.Facade;
using UnityEngine;

namespace Features.XR
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "ScriptableObjects/XR/MovementSettings")]
    public class MovementSettings_SO : ScriptableObject
    {
        [field: SerializeField] public Setting<bool> TurnProvider { get; private set; } = new(false);
        [field: SerializeField] public ClampedSetting ContinuousTurnSpeed { get; private set; } = new(30, 30, 70);
        [field: SerializeField] public ClampedSetting SnapTurnDegrees { get; private set; } = new(30, 30, 60);
        [field: SerializeField] public ClampedSetting MovementSpeed { get; private set; } = new(2.5f, 2.5f, 3.5f);

        private void OnDestroy()
        {
            TurnProvider.Dispose();
            ContinuousTurnSpeed.Dispose();
            SnapTurnDegrees.Dispose();
            MovementSpeed.Dispose();
        }
    }

    public static class MovementSettingsExtensions
    {
        public static void Save(this MovementSettings_SO movementSettings)
        {
            PlayerPrefs.SetInt(
                nameof(movementSettings.TurnProvider), movementSettings.TurnProvider.Value ? 1 : 0);
            PlayerPrefs.SetFloat(
                nameof(movementSettings.ContinuousTurnSpeed), movementSettings.ContinuousTurnSpeed.Value);
            PlayerPrefs.SetFloat(
                nameof(movementSettings.MovementSpeed), movementSettings.MovementSpeed.Value);
            PlayerPrefs.SetFloat(
                nameof(movementSettings.SnapTurnDegrees), movementSettings.SnapTurnDegrees.Value);
        }
        
        public static void Load(this MovementSettings_SO movementSettings)
        {
            movementSettings.TurnProvider.Value 
                = PlayerPrefs.GetInt(nameof(movementSettings.TurnProvider), 1) == 1;
            movementSettings.ContinuousTurnSpeed.Value 
                = PlayerPrefs.GetFloat(nameof(movementSettings.ContinuousTurnSpeed), 60);
            movementSettings.MovementSpeed.Value 
                = PlayerPrefs.GetFloat(nameof(movementSettings.MovementSpeed), 3);
            movementSettings.SnapTurnDegrees.Value 
                = PlayerPrefs.GetFloat(nameof(movementSettings.SnapTurnDegrees), 45);
        }
    }
}