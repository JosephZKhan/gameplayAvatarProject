using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hoverMeterScript : MonoBehaviour
{

    public Slider slider;

    public void setMaxValue(float maxVal)
    {
        slider.maxValue = maxVal;
        slider.value = 0;
    }

    public void setValue(float val)
    {
        slider.value = slider.maxValue - val;
    }
}
