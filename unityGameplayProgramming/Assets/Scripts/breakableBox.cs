using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakableBox : MonoBehaviour
{
    GameObject playerRef;
    Collider playerPunch;
    playerController2 playerScriptRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerPunch = playerRef.GetComponent<BoxCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerPunch && other.enabled)
        {
            playerScriptRef.triggerPunchEffect();
            playerScriptRef.setCanMoveForward(true);
            playerScriptRef.setCanMoveBackward(true);
            Destroy(gameObject);
        }
    }





}
