using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ringMovement : MonoBehaviour
{

    public float rotationSpeed = 1.0f;

    GameObject playerRef;
    Collider playerColliderRef;
    playerController2 playerScriptRef;

    private void Awake()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerColliderRef = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0.0f, rotationSpeed, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == playerColliderRef)
        {
            //playerScriptRef.doubleJumpPowerUp();
            playerScriptRef.collectRing();
            Destroy(gameObject);
        }
    }
}
