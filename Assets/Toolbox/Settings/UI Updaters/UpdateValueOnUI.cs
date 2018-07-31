using UnityEngine;
using System.Collections;

public class UpdateValueOnUI : MonoBehaviour
{

    #region MonoBehaviour Messages

    // Use this for initialization
    void Start()
    {
        UpdateValue();
    }

    #endregion


    #region Overridable Methods

    public virtual void UpdateValue() { }

    #endregion

}
