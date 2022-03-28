using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class platformBehaviour : MonoBehaviour
{
    Collider coll;

    //public enum despawnCondition { despawnOnHit, despawnOnHitRespawn, blink };

    public bool disappearsOnHit = false;
    public float timeToDisappear = 4.0f;

    public bool respawns = false;
    public float timeToRespawn = 6.0f;

    public bool blinking = false;
    public float blinkInterval = 5.0f;
    bool blinkActive = false;

    public bool growOnHit = false;
    public enum axis { X, Y, Z };
    public axis growAxis;
    public float growthMultiplier = 2.5f;
    Vector3 finalSize;
    public float timeToGrow = 5.0f;

    GameObject playerRef;
    Collider playerColl;

    Transform surface;

    // Start is called before the first frame update
    void Awake()
    {
        coll = GetComponent<BoxCollider>();

        playerRef = GameObject.FindWithTag("Player");
        playerColl = playerRef.GetComponent<CapsuleCollider>();

        surface = transform.Find("Plane");

        if (blinking)
        {
            disappearsOnHit = false;
            respawns = false;
        }

        if (growAxis == axis.X)
        {
            finalSize = new Vector3(transform.localScale.x * growthMultiplier, transform.localScale.y, transform.localScale.z);
        }
        if (growAxis == axis.Y)
        {
            finalSize = new Vector3(transform.localScale.x, transform.localScale.y * growthMultiplier, transform.localScale.z);
        }
        if (growAxis == axis.Z)
        {
            finalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * growthMultiplier);
        }
    }

    private void Start()
    {
        if (blinking)
        {
            StartCoroutine(blink());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == playerColl)
        {
            Debug.Log("touching platform");
            /*if (Physics.BoxCast(transform.position, transform.localScale, Vector3.up, transform.rotation, Mathf.Infinity))
            {

            }*/

            if (playerRef.transform.position.y >= surface.transform.position.y)
            {
                Debug.Log("standing on top");
                if (disappearsOnHit)
                {
                    StartCoroutine(triggerDespawn());
                }
                if (growOnHit)
                {
                    StartCoroutine(grow());
                }
            }
            
            /*if (Physics.Raycast(coll.transform.position, Vector3.up, 100.1f))
            {
                *//*Debug.Log("standing on top");
                if (disappearsOnHit)
                {
                    StartCoroutine(triggerDespawn());
                }
                if (growOnHit)
                {
                    StartCoroutine(grow());
                }*//*
            }*/
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
        setExistence(false);
        if (respawns)
        {
            StartCoroutine(triggerRespawn());
        }
    }

    private void respawn()
    {
        setExistence(true);
    }

    IEnumerator blink()
    {
        yield return new WaitForSeconds(blinkInterval);
        blinkActive = !blinkActive;
        setExistence(blinkActive);
        StartCoroutine(blink());
    }

    private void setExistence(bool existence)
    {
        gameObject.GetComponent<BoxCollider>().enabled = existence;
        gameObject.GetComponent<MeshRenderer>().enabled = existence;
        surface.GetComponent<MeshRenderer>().enabled = existence;
        surface.GetComponent<MeshCollider>().enabled = existence;
    }

    IEnumerator grow()
    {
        float elapsedTime = 0;
        float waitTime = timeToGrow;


        //currentPosition = transform.position;
        Vector3 currentScale = transform.localScale;

        while (elapsedTime < waitTime)
        {
            //transform.position = Vector3.Lerp(currentPosition, endPos, (elapsedTime / waitTime));
            transform.localScale = Vector3.Lerp(currentScale, finalSize, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = finalSize;
        yield return null;
    }
}
