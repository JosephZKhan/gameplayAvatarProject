using System;
using UnityEngine;

[Serializable]
public struct OptionalBlink<T, S>
{

    [SerializeField] private bool enabled;
    [SerializeField] private T time;
    [SerializeField] private S active;



    public OptionalBlink(T initialValue, S startActive)
    {
        enabled = true;
        time = initialValue;
        active = startActive;
    }

    public bool Enabled => enabled;
    public T Time => time;
    public S Active => active;

    public void toggle(S newToggle)
    {
        active = newToggle;
    }

}
