using System;
using UnityEngine;
using PathCreation;

[Serializable]
public struct OptionalMove<S, E, M>
{

    [SerializeField] private bool enabled;
    [SerializeField] private S speed;
    [SerializeField] private E end;
    [SerializeField] private M mode;



    public OptionalMove(S initialSpeed, E initialEnd, M initialMode)
    {
        enabled = true;
        speed = initialSpeed;
        end = initialEnd;
        mode = initialMode;
    }

    public bool Enabled => enabled;
    public S Speed => speed;
    public E End => end;
    public M Mode => mode;

}
