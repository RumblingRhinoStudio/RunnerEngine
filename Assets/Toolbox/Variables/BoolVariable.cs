using UnityEngine;

[CreateAssetMenu(fileName = "BoolVariable", menuName = "Toolbox/Variables/Bool Variable", order = 0)]
public class BoolVariable : ScriptableObject {

    #if UNITY_EDITOR

    [TextArea]
    public string DeveloperDescription = "";

    #endif

    public bool Value;

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(BoolVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(bool amount)
    {
        Value = amount;
    }

    public void ApplyChange(BoolVariable amount)
    {
        Value = amount.Value;
    }

}
