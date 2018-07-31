using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UpdateValueOnSlider : UpdateValueOnUI
{

    #region Properties

    private Slider _slider;
    private Slider slider
    {
        get { return _slider; }
        set { _slider = value; }
    }

    [SerializeField]
    private FloatReference _value;
    private FloatReference value
    {
        get { return _value; }
    }

    #endregion


    #region MonoBehaviour Messages

    private void Awake()
    {
        slider = gameObject.GetComponent<Slider>();
    }

    #endregion


    #region Public Methods

    public override void UpdateValue()
    {
        slider.value = value.UseConstant ? value.ConstantValue : value.Variable.Value;
    }

    #endregion

}
