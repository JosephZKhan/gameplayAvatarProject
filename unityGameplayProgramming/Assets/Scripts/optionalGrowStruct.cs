using System;
using UnityEngine;

[Serializable]
public struct OptionalGrow<T, S, U>
{

    [SerializeField] private bool enabled;
    [SerializeField] private T time;
    [SerializeField] private S scaleFactor;
    [SerializeField] private U axis;



    public OptionalGrow(T initialTime, S initialScale, U initialAxis)
    {
        enabled = true;
        time = initialTime;
        scaleFactor = initialScale;
        axis = initialAxis;

        //active = startActive;
    }

    public bool Enabled => enabled;
    public T Time => time;
    public S ScaleFactor => scaleFactor;
    public U Axis => axis;

}
