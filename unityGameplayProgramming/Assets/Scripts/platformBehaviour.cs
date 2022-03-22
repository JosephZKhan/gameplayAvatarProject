using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformBehaviour : MonoBehaviour
{

    public bool disappears = false;
    public float timeToDisappear = 4.0f;

    public bool respawns = false;
    public float timeToRespawn = 6.0f;

    GameObject playerRef;
    Collider playerColl;

    Transform surface;

    // Start is called before the first frame update
    void Awake()
    {
        playerRef = GameObject.FindWithTag("Player");
        playerColl = playerRef.GetComponent<CapsuleCollider>();

        surface = transform.Find("Plane");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == playerColl)
        {
            if (disappears)
            {
                StartCoroutine(triggerDespawn());
            }
        }
    }

    IEnumerator triggerDespawn()
    {
        yield return new WaitForSeconds(timeToDisappear);
        despawn();
    }

    IEnumerator triggerRespawn()
    {
        yield return new WaitForSeconds(timeToRespawn);
        respawn();
    }

    private void despawn()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        surface.GetComponent<MeshRenderer>().enabled = false;
        surface.GetComponent<MeshCollider>().enabled = false;
        if (respawns)
        {
            StartCoroutine(triggerRespawn());
        }
    }

    private void respawn()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        surface.GetComponent<MeshRenderer>().enabled = true;
        surface.GetComponent<MeshCollider>().enabled = true;
    }
}
