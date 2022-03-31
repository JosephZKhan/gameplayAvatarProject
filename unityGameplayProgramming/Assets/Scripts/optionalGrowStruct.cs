using System;
using UnityEngine;

[Serializable]
public struct OptionalGrow<T, S, U, M, D>
{

    [SerializeField] private bool enabled;
    [SerializeField] private T time;
    [SerializeField] private S scaleFactor;
    [SerializeField] private U axis;
    [SerializeField] private M mode;
    [SerializeField] private D delay;



    public OptionalGrow(T initialTime, S initialScale, U initialAxis, M initialMode, D initialDelay)
    {
        enabled = true;
        time = initialTime;
        scaleFactor = initialScale;
        axis = initialAxis;
        mode = initialMode;
        delay = initialDelay;

        //active = startActive;
    }

    public bool Enabled => enabled;
    public T Time => time;
    public S ScaleFactor => scaleFactor;
    public U Axis => axis;
    public M Mode => mode;
    public D Delay => delay;

}
