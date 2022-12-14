using System;
using UnityEngine;


[CreateAssetMenu(fileName = "ConceptSO", menuName = "Scriptable Objects/Settings/Concept")]
public class Concept_SO : ScriptableObject
{
    public event Action<float> OnSetting1Changed;
    public event Action<string> OnSetting2Changed;
    public event Action<bool> OnSetting3Changed;

    private float _setting1;
    private string _setting2;
    private bool _setting3;

    public float Setting1
    {
        get => _setting1;
        set
        {
            _setting1 = value;
            OnSetting1Changed?.Invoke(value);
        }
    }

    public string Setting2
    {
        get => _setting2;
        set
        {
            _setting2 = value;
            OnSetting2Changed?.Invoke(value);
        }
    }

    public bool Setting3
    {
        get => _setting3;
        set
        {
            _setting3 = value;
            OnSetting3Changed?.Invoke(value);
        }
    }
    
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(nameof(Setting1), Setting1);
        PlayerPrefs.SetString(nameof(Setting2), Setting2);
        PlayerPrefs.SetInt(nameof(Setting3), Setting3 ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        Setting1 = PlayerPrefs.GetFloat(nameof(Setting1), 0f );
        Setting2 = PlayerPrefs.GetString(nameof(Setting2), string.Empty );
        Setting3 = PlayerPrefs.GetInt(nameof(Setting3), 0) == 1;
    }
}
