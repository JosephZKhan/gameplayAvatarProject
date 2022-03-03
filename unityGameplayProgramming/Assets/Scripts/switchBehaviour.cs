using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class switchBehaviour : MonoBehaviour
{

    [SerializeField] Collider playerColl;
    [SerializeField] playerController2 playerScriptRef;
    [SerializeField] PlayableDirector cutscene;

    Light spotlight;

    void Awake()
    {
        spotlight = gameObject.transform.GetChild(2).gameObject.GetComponent<Light>();
    }




    void OnTriggerEnter(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(true);
            playerScriptRef.setTargetSwitch(this);

            spotlight.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(false);

            spotlight.enabled = false;
        }
    }

    public void Activate()
    {
        cutscene.Play();
    }
}
