using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthCounterUI : MonoBehaviour
{
    Text text;

    public void Awake()
    {
        text = GetComponent<Text>();
    }

    public void setCounter(int newRingCount)
    {
        newRingCount = Mathf.Clamp(newRingCount, 0, 100);
        text.text = "HP - " + newRingCount.ToString();
    }
}
