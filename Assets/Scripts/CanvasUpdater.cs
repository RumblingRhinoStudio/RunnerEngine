using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUpdater : MonoBehaviour
{
    public FloatVariable ValueToDisplay;
    private Text text;

    void Start()
    {
        text = GetComponent<Text>(); 
    }

    void Update()
    {
        text.text = ValueToDisplay.Value.ToString();
    }
}
