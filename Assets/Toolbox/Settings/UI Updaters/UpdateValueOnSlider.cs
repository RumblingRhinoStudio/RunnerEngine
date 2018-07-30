using UnityEngine;
using UnityEngine.UI;

public class UpdateValueOnSlider : MonoBehaviour
{

    #region Properties

    [SerializeField]
    private Slider _slider;
    private Slider slider
    {
        get { return _slider; }
    }

    [SerializeField]
    private FloatReference _value;
    private FloatReference value
    {
        get { return _value; }
    }

    #endregion


    #region MonoBehaviour Messages

    private void Start()
    {
        UpdateValue();
    }

    #endregion


    #region Public Methods

    public void UpdateValue()
    {
        slider.value = value.UseConstant ? value.ConstantValue : value.Variable.Value;
    }

    #endregion

}
