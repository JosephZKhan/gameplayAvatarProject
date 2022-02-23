using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedPowerUp : MonoBehaviour
{

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

    void OnTriggerEnter(Collider other)
    {
        if (other == playerColliderRef)
        {
            playerScriptRef.speedPowerUp();
            Destroy(gameObject);
        }
    }
}
