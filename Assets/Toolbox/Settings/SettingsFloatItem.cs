using System;
using UnityEngine;
using System.Collections;

/*
 * Designers should be able to:
 * - Set up the UI
 * - Define the setting items name
 * - Select the type of setting (checkbox, slider, etc.)
 * - CURRENTLY DOING: Setting GameEvent up so it uses UnityEvent.
 */

[CreateAssetMenu(fileName = "Settings Item", menuName = "Settings/Settings Float Item", order = 0)]
public class SettingsFloatItem : SettingsItem
{

    #region Properties

    [SerializeField]
    private FloatReference _value;
    public FloatReference Value
    {
        get { return _value; }
    }

    #endregion


    #region Public Methods

    public void SetValue(float value)
    {
        _value.Variable.SetValue(value);        
        Database.SaveString(databaseKeyName, value.ToString());
        valueChangedEvent.Raise();
    }

    public void LoadPersistedValue()
    {
        string persistedValue = Database.GetString(databaseKeyName);
        Value.Variable.Value = Value.ConstantValue = float.Parse(!string.IsNullOrWhiteSpace(persistedValue) ? persistedValue : "0");
    }

    #endregion


    #region MonoBehaviour Messages



    #endregion

}