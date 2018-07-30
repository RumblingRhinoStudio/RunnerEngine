using UnityEngine;

public abstract class Database : ScriptableObject {

    public abstract void SaveString(string key, string value);

    public abstract string GetString(string key);

}
