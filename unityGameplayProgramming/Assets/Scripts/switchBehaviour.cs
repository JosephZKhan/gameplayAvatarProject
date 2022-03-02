using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class switchBehaviour : MonoBehaviour
{

    [SerializeField] Collider playerColl;
    [SerializeField] playerController2 playerScriptRef;
    [SerializeField] PlayableDirector cutscene;




    void OnTriggerEnter(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(true);
            playerScriptRef.setTargetSwitch(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(false);
        }
    }

    public void Activate()
    {
        cutscene.Play();
    }
}
