using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class switchBehaviour : MonoBehaviour
{

    [SerializeField] Collider playerColl;
    [SerializeField] playerController2 playerScriptRef;
    [SerializeField] PlayableDirector cutscene;
    [SerializeField] PlayableDirector resetCutscene;
    [SerializeField] Text resetTimerText;
    [SerializeField] GameObject powerup;

    Light spotlight;
    //GameObject button;

    bool isTriggered = false;

    public bool hasReset = false;
    public int timeUntilReset = 10;

    public bool powerupSpawner = false;


    void Awake()
    {
        spotlight = gameObject.transform.GetChild(2).gameObject.GetComponent<Light>();
        //button = gameObject.transform.GetChild(0).gameObject;

    }

    void Update()
    {
        if (powerupSpawner)
        {
            if (cutscene.state != PlayState.Playing)
            {
                if (isTriggered && !powerup.active)
                {
                    Debug.Log("bababooey");
                    resetCutscene.Play();
                    isTriggered = false;
                }
            }
        }
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
                StartCoroutine(reset(timeUntilReset));
            }
        }
    }

    IEnumerator reset(int timeRemaining)
    {
        for (int i = timeRemaining; i > 0; i--)
        {
            resetTimerText.text = i.ToString("00");
            yield return new WaitForSeconds(1);
        }
        if (hasReset)
        {
            resetCutscene.Play();
            isTriggered = false;
        }
        
    }
}
