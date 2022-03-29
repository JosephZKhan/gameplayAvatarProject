using System;
using UnityEngine;
using PathCreation;

[Serializable]
public struct OptionalMove<S, E>
{

    [SerializeField] private bool enabled;
    [SerializeField] private S speed;
    [SerializeField] private E end;



    public OptionalMove(S initialSpeed, E initialEnd)
    {
        enabled = true;
        speed = initialSpeed;
        end = initialEnd;
    }

    public bool Enabled => enabled;
    public S Speed => speed;
    public E End => end;

}
