using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchPlayerDetection : MonoBehaviour
{

    [SerializeField] Collider playerColl;
    [SerializeField] playerController2 playerScriptRef;



    void OnTriggerEnter(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(false);
        }
    }
}
