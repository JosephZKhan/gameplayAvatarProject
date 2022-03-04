using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class switchBehaviour : MonoBehaviour
{

    [SerializeField] Collider playerColl;
    [SerializeField] playerController2 playerScriptRef;
    [SerializeField] PlayableDirector cutscene;
    [SerializeField] PlayableDirector resetCutscene;

    Light spotlight;
    //GameObject button;

    bool isTriggered = false;

    public bool hasReset = false;
    public float timeUntilReset = 10.0f;



    void Awake()
    {
        spotlight = gameObject.transform.GetChild(2).gameObject.GetComponent<Light>();
        //button = gameObject.transform.GetChild(0).gameObject;

    }




    void OnTriggerEnter(Collider other)
    {
        if (other == playerColl)
        {
            playerScriptRef.setInSwitchCollider(true);
            playerScriptRef.setTargetSwitch(this);

            if (!isTriggered)
            {
                spotlight.enabled = true;
            }
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
        if (!isTriggered)
        {
            cutscene.Play();
            isTriggered = true;
            if (hasReset)
            {
                StartCoroutine(reset());
            }
        }
    }

    IEnumerator reset()
    {
        yield return new WaitForSeconds(timeUntilReset);
        if (hasReset)
        {
            Debug.Log("reset!");
            resetCutscene.Play();
            isTriggered = false;
        }
        
    }
}
