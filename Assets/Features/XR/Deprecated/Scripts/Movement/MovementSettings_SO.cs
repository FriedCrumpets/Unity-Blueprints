using System.Collections.Generic;
using Blueprints.DoD;
using Blueprints.Facade;
using UnityEngine;

namespace Features.XR
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "ScriptableObjects/XR/MovementSettings")]
    public class MovementSettings_SO : ScriptableObject
    {
        [field: SerializeField] public IData<bool> TurnProvider { get; private set; } = new Data<bool>();

        [field: SerializeField] public IDataSet<float> ContinuousTurnSpeed { get; private set; } =
            new ClampedDataSet(30, 30, 70);
        
        [field: SerializeField] public IDataSet<float> SnapTurnDegrees { get; private set; } =
            new ClampedDataSet(30, 30, 60);
        
        [field: SerializeField] public IDataSet<float> MovementSpeed { get; private set; } =
            new ClampedDataSet(2.5f, 2.5f, 3.5f);
    }

    public static class MovementSettingsExtensions
    {
        public static void Save(this MovementSettings_SO movementSettings)
        {
            PlayerPrefs.SetInt(
                nameof(movementSettings.TurnProvider), movementSettings.TurnProvider.Get() ? 1 : 0);
            PlayerPrefs.SetFloat(
                nameof(movementSettings.ContinuousTurnSpeed), movementSettings.ContinuousTurnSpeed.Read("value"));
            PlayerPrefs.SetFloat(
                nameof(movementSettings.MovementSpeed), movementSettings.MovementSpeed.Read("value"));
            PlayerPrefs.SetFloat(
                nameof(movementSettings.SnapTurnDegrees), movementSettings.SnapTurnDegrees.Read("value"));
        }
        
        public static void Load(this MovementSettings_SO movementSettings)
        {
            movementSettings.TurnProvider.Set(
                PlayerPrefs.GetInt(nameof(movementSettings.TurnProvider), 1) == 1);
            movementSettings.ContinuousTurnSpeed.Set( 
                "value", PlayerPrefs.GetFloat(nameof(movementSettings.ContinuousTurnSpeed), 60));
            movementSettings.MovementSpeed.Set( 
                "value", PlayerPrefs.GetFloat(nameof(movementSettings.MovementSpeed), 3));
            movementSettings.SnapTurnDegrees.Set( 
                "value", PlayerPrefs.GetFloat(nameof(movementSettings.SnapTurnDegrees), 45));
        }
    }
}