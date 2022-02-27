using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fillingMeterScript : MonoBehaviour
{

    public Slider slider;
    public GameObject background;

    private void Awake()
    {
        background.SetActive(false);
    }

    public void setMaxValue(float maxVal)
    {
        slider.maxValue = maxVal;
        slider.value = 0;
    }

    public void setValue(float val)
    {
        slider.value = slider.maxValue - val;
    }

    public void spawnBackground()
    {
        background.SetActive(true);
    }

    public void despawnBackground()
    {
        background.SetActive(false);
    }
}
