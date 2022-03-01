using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ringCounterUI : MonoBehaviour
{

    Text text;

    public void Awake()
    {
        text = GetComponent<Text>();
    }

    public void setCounter(int newRingCount)
    {
        text.text = "RINGS - " + newRingCount.ToString();
    }

}
