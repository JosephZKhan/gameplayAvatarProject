using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splineFieldCollider : MonoBehaviour
{

    GameObject playerRef;
    Collider playerColl;
    playerController2 playerScriptRef;

    // Start is called before the first frame update
    void Awake()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerColl = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other == playerColl)
        {
            if (playerScriptRef.getTargetSpline() != null)
            {
                playerScriptRef.setOnSpline(true);

                //Debug.Log("on spline");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setOnSpline(false);

            Debug.Log("off spline");
        }
        
    }
}
