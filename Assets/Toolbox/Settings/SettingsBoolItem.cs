using UnityEngine;
using System.Collections;

/*
 * Designers should be able to:
 * - Set up the UI
 * - Define the setting items name
 * - Select the type of setting (checkbox, slider, etc.)
 */

[CreateAssetMenu(fileName = "Settings Item", menuName = "Settings/Settings Bool Item", order = 0)]
public class SettingsBoolItem : SettingsItem
{

    [SerializeField]
    private FloatReference _value;
    public FloatReference Value
    {
        get { return _value; }
    }

}
