using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPrefs Database", menuName = "Databases/PlayerPrefs Database", order = 0)]
public class PlayerPrefsDatabase : Database
{
    public override string GetString(string key)
    {
        return PlayerPrefs.GetString(key);
    }

    public override void SaveString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }
}
