using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class splineEntryPoint : MonoBehaviour
{

    public bool isStart;

    GameObject playerRef;
    Collider playerColl;
    playerController2 playerScriptRef;

    [SerializeField] PathCreator targetPath;

    void Awake()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerColl = playerRef.GetComponent<CapsuleCollider>();
        playerScriptRef = playerRef.GetComponent<playerController2>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setTargetSpline(targetPath);
            if (isStart)
            {
                playerScriptRef.setSplinePoint(0);
            }
        }

    }

    /*void OnTriggerExit(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setOnSpline(false);

            Debug.Log("off spline");
        }
    }*/
}
