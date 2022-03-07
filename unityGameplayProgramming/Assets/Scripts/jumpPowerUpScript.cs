using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpPowerUpScript : MonoBehaviour
{
    GameObject playerRef;
    Collider playerColliderRef;
    playerController2 playerScriptRef;
    ParticleSystem sparkles;


    void Awake()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerColliderRef = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
        sparkles = GetComponentInChildren<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerColliderRef)
        {
            playerScriptRef.doubleJumpPowerUp();
            Destroy(sparkles);
            gameObject.SetActive(false);
        }
    }
}
