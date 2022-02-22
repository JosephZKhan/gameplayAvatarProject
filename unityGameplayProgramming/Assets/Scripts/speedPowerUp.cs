using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedPowerUp : MonoBehaviour
{

    public float rotationSpeed;
    GameObject playerRef;
    Collider playerColliderRef;
    playerController2 playerScriptRef;


    void Awake()
    {
        playerRef = GameObject.FindWithTag("Player");
        Debug.Log(playerRef);
        playerColliderRef = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
        Debug.Log(playerScriptRef);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, rotationSpeed, 0.0f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerColliderRef)
        {
            playerScriptRef.speedPowerUp();
            Destroy(gameObject);
        }
    }
}
