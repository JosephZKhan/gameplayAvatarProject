using System;
using UnityEngine;

[Serializable]
public struct OptionalGrow<T, S, U, M>
{

    [SerializeField] private bool enabled;
    [SerializeField] private T time;
    [SerializeField] private S scaleFactor;
    [SerializeField] private U axis;
    [SerializeField] private M mode;



    public OptionalGrow(T initialTime, S initialScale, U initialAxis, M initialMode)
    {
        enabled = true;
        time = initialTime;
        scaleFactor = initialScale;
        axis = initialAxis;
        mode = initialMode;

        //active = startActive;
    }

    public bool Enabled => enabled;
    public T Time => time;
    public S ScaleFactor => scaleFactor;
    public U Axis => axis;
    public M Mode => mode;

}
