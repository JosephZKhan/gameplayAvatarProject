using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineObstacle : MonoBehaviour
{

    GameObject playerRef;
    Collider playerColl;
    playerController2 playerScriptRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerColl = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerColl && other.enabled)
        {
            float playerMoveY = playerScriptRef.getMovement().y;

            if (playerMoveY > 0)
            {
                playerScriptRef.setCanMoveForward(false);
            }
            if (playerMoveY < 0)
            {
                playerScriptRef.setCanMoveBackward(false);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerColl && other.enabled)
        {
            float playerMoveY = playerScriptRef.getMovement().y;

            playerScriptRef.setCanMoveForward(true);
            playerScriptRef.setCanMoveBackward(true);

        }
    }
}
