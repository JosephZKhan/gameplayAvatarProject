using System;
using UnityEngine;

[Serializable]
public struct OptionalRotate<S, A, M>
{

    [SerializeField] private bool enabled;
    [SerializeField] private S speed;
    [SerializeField] private A axis;
    [SerializeField] private M mode;



    public OptionalRotate(S initialSpeed, A initialAxis, M initialMode)
    {
        enabled = true;
        speed = initialSpeed;
        axis = initialAxis;
        mode = initialMode;
    }

    public bool Enabled => enabled;
    public S Speed => speed;
    public A Axis => axis;
    public M Mode => mode;

}
