using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerLockOn : MonoBehaviour
{

    Collider lockOnColl;
    playerController2 playerScriptRef;
    [SerializeField] controlCamera cameraControllerRef;

    int noOfTargets;

    // Start is called before the first frame update
    void Awake()
    {
        lockOnColl = GetComponent<SphereCollider>();
        playerScriptRef = GetComponent<playerController2>();
    }

    private void Update()
    {
        if (noOfTargets <= 0)
        {
            playerScriptRef.disableLockOn();
            cameraControllerRef.disableLockOn();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Punchable")
        {
            if (!playerScriptRef.getIsLockedOn())
            {
                playerScriptRef.assignLockOnTarget(other.gameObject);
                cameraControllerRef.assignLockOnTarget(other.gameObject);
                noOfTargets++;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Punchable")
        {
            if (!playerScriptRef.getIsLockedOn())
            {
                noOfTargets--;
            }
            //noOfTargets--;
        }
    }
}
