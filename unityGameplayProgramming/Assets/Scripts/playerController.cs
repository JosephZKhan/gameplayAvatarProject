using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{

    private Rigidbody rb;
    private float movementX;
    private float movementY;
    public float speed = 1;
    private float maxVelocityX = 50000;
    private float maxVelocityY = 50000;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (rb.velocity.x >= maxVelocityX)
        {
            Debug.Log("TOO FAST IN X");
            rb.velocity = new Vector3(maxVelocityX, rb.velocity.y, rb.velocity.z);
        }

        if (rb.velocity.y >= maxVelocityY)
        {
            Debug.Log("TOO FAST IN Y");
            rb.velocity = new Vector3(rb.velocity.x, maxVelocityY, rb.velocity.z);
        }



    }
}
