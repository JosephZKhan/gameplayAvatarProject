using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generalPowerupMovement : MonoBehaviour
{
    public float movementSpeed;
    public float movementRange;

    public float rotationSpeed;


    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0.0f, rotationSpeed, 0.0f);

        //if (|transform.y| > movementRange)
        if (Mathf.Abs(transform.localPosition.y) > movementRange)
        {
            movementSpeed = -movementSpeed;
        }
        transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
    }
}
