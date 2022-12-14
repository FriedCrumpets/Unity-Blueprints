using Blueprints.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

public class Implementer : MonoBehaviour
{
    [SerializeField] private Concept settings;
    [SerializeField] private InputActionReference input;

    private void Start()
    {
        settings.storage.LoadSettings();
    }

    private void OnDestroy()
    {
        settings.storage.SaveSettings();
    }

    private void OnEnable()
    {
        input.action.Enable();
        input.action.started += IncrementValue;
        settings.storage.OnSetting1Changed += PrintChange;
    }

    private void OnDisable()
    {
        input.action.Disable();
        input.action.started -= IncrementValue;
        settings.storage.OnSetting1Changed -= PrintChange;
    }

    private void IncrementValue(InputAction.CallbackContext context) => settings.Setting1 += 1;

    private void PrintChange(float value) => print(value);
   
}
