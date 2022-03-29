using System;
using UnityEngine;

[Serializable]
public struct Optional<T>
{

    [SerializeField] private bool disappearsOnHit;
    [SerializeField] private T timeToDisappear;

    public Optional(T initialValue)
    {
        disappearsOnHit = true;
        timeToDisappear = initialValue;
    }

    /*[SerializeField] private bool respawns = false;
    [SerializeField] private float timeToRespawn = 6.0f;*/

    public bool DisappearsOnHit => disappearsOnHit;
    public T TimeToDisappear => timeToDisappear;

}