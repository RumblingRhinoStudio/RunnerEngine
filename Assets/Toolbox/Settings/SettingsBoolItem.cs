using UnityEngine;
using System.Collections;

/*
 * Designers should be able to:
 * - Set up the UI
 * - Define the setting items name
 * - Select the type of setting (checkbox, slider, etc.)
 */

[CreateAssetMenu(fileName = "SettingsBoolItem", menuName = "Settings/Settings Bool Item", order = 0)]
public class SettingsBoolItem : SettingsItem<bool>
{

    #region Properties

    [SerializeField]
    private BoolReference _value;
    public BoolReference Value
    {
        get { return _value; }
    }

    #endregion


    #region Public Methods

    public override void SetValue(bool value)
    {
        _value.Variable.SetValue(value);
        Database.SaveString(databaseKeyName, value.ToString().ToUpper());
        valueChangedEvent.Raise();
    }

    public override void LoadPersistedValue()
    {
        string persistedValue = Database.GetString(databaseKeyName);
        Value.Variable.Value = Value.ConstantValue = !string.IsNullOrWhiteSpace(persistedValue) ? 
            persistedValue.ToUpper() == "TRUE" ? true : false : 
            false;
    }

    #endregion

}
