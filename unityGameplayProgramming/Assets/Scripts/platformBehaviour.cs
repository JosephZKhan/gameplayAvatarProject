using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class platformBehaviour : MonoBehaviour
{
    Collider coll;

    public enum axis { X, Y, Z };

    [Header("Designer settings")]
    [SerializeField] private Optional<float> Despawn = new Optional<float>(4.0f);
    [SerializeField] private Optional<float> Respawn = new Optional<float>(4.0f);
    [SerializeField] private OptionalBlink<float, bool> Blink = new OptionalBlink<float, bool>(2.5f, false);
    [SerializeField] private OptionalGrow<float, float, axis> Grow = new OptionalGrow<float, float, axis>(3.0f, 6.5f, axis.Z);
    //[SerializeField] private OptionalMove<axis, float> Move = new OptionalMove<axis, float>(axis.X, 3.5f);
    [SerializeField] private OptionalMove<float, EndOfPathInstruction> Move = new OptionalMove<float, EndOfPathInstruction>(1.5f, EndOfPathInstruction.Reverse);

    //PathCreator movePath;
    //public float pathSpeed;
    float pathPoint;
    //public EndOfPathInstruction end;

    Vector3 finalSize;

    [SerializeField] private Transform surface;
    [SerializeField] private PathCreator movePath;

    bool playerOnTop = false;

    void Awake()
    {
        coll = GetComponent<BoxCollider>();

        //movePath = GetComponentInChildren<PathCreator>();
    }

    private void Start()
    {
        if (Blink.Enabled)
        {
            StartCoroutine(blink());
        }
    }

    private void Update()
    {
        if (playerOnTop)
        {
            if (Despawn.Enabled)
            {
                StartCoroutine(triggerDespawn());
            }
            if (Grow.Enabled)
            {
                StartCoroutine(grow());
            }
        }

        if (Move.Enabled)
        {
            pathPoint += Move.Speed * Time.deltaTime;
            //Vector3 platformPathPos = new Vector3(movePath.path.GetPointAtDistance(pathPoint, end).x, movePath.path.GetPointAtDistance(pathPoint).y, movePath.path.GetPointAtDistance(pathPoint).z);
            transform.position = new Vector3(movePath.path.GetPointAtDistance(pathPoint, Move.End).x, movePath.path.GetPointAtDistance(pathPoint, Move.End).y, movePath.path.GetPointAtDistance(pathPoint, Move.End).z);
            transform.eulerAngles = new Vector3(0, movePath.path.GetRotationAtDistance(pathPoint, Move.End).eulerAngles.y, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.CompareTag("Player"))
        {
            // playerController2 t = collision.collider.GetComponent<playerController2>();
            collision.collider.transform.parent = transform;
        }*/

        if (collision.collider.CompareTag("Player"))
        {

            if (collision.collider.transform.position.y >= surface.transform.position.y)
            {
                playerOnTop = true;

                if (Move.Enabled)
                {
                    collision.collider.transform.parent = transform;
                }
            }
            else
            {
                playerOnTop = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // playerController2 t = collision.collider.GetComponent<playerController2>();
            collision.collider.transform.parent = null;
        }
    }

    IEnumerator triggerDespawn()
    {
        yield return new WaitForSeconds(Despawn.Time);
        despawn();
    }

    IEnumerator triggerRespawn()
    {
        yield return new WaitForSeconds(Respawn.Time);
        respawn();
    }

    private void despawn()
    {
        setExistence(false);

        if (Respawn.Enabled)
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
        yield return new WaitForSeconds(Blink.Time);
        Blink.toggle(!Blink.Active);
        setExistence(Blink.Active);
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
        float waitTime = Grow.Time;

        Vector3 currentScale = transform.localScale;

        

        if (Grow.Axis == axis.X)
        {
            finalSize = new Vector3(transform.localScale.x * Grow.ScaleFactor, transform.localScale.y, transform.localScale.z);
        }
        if (Grow.Axis == axis.Y)
        {
            finalSize = new Vector3(transform.localScale.x, transform.localScale.y * Grow.ScaleFactor, transform.localScale.z);
        }
        if (Grow.Axis == axis.Z)
        {
            finalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * Grow.ScaleFactor);
        }

        while (elapsedTime < waitTime)
        {
            transform.localScale = Vector3.Lerp(currentScale, finalSize, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = finalSize;
        yield return null;
    }
}