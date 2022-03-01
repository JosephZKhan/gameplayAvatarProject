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
        playerColliderRef = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerColliderRef)
        {
            playerScriptRef.speedPowerUp();
            gameObject.SetActive(false);
        }
    }

    IEnumerator respawn()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(5.0f);
        Debug.Log("hello");
    }

}
