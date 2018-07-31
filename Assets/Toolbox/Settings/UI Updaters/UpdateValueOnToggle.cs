using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Toggle))]
public class UpdateValueOnToggle : UpdateValueOnUI
{

    private Toggle _toggle;
    private Toggle toggle
    {
        get { return _toggle; }
        set { _toggle = value; }
    }

    [SerializeField]
    private BoolReference _value;
    private BoolReference value
    {
        get { return _value; }
    }

    #region MonoBehaviour Messages

    private void Awake()
    {
        toggle = gameObject.GetComponent<Toggle>();
    }

    #endregion


    #region Public Methods

    public override void UpdateValue()
    {
        toggle.isOn = value.UseConstant ? value.ConstantValue : value.Variable.Value;
    }

    #endregion
}
