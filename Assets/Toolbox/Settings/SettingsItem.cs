using UnityEngine;
using System.Collections;

/*
 * Designers should be able to:
 * - Set up the UI
 * - Define the setting items name
 * - Select the type of setting (checkbox, slider, etc.)
 */

[CreateAssetMenu(fileName = "Settings Item", menuName = "Settings/Settings Item", order = 0)]
public class SettingsItem : ScriptableObject
{
    #region Properties

    [SerializeField]
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    [SerializeField]
    private Database _database;
    public Database Database
    {
        get { return _database; }
    }

    [SerializeField]
    private string _databaseKeyName;
    protected string databaseKeyName
    {
        get { return _databaseKeyName; }
    }

    #endregion


    #region GameEvents

    [SerializeField]
    protected GameEvent valueChangedEvent;

    [SerializeField]
    protected GameEvent valueLoadedEvent;

    #endregion
}
