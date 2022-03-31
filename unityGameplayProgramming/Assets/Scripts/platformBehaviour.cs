using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class platformBehaviour : MonoBehaviour
{
    Collider coll;

    public enum axis { X, Y, Z };
    public enum movement { Automatic, OneWay, Reverse };
    public enum growth { Automatic, OneWay, Reverse };
    private enum rotation { Automatic, StartOnStand, TriggerOnStand };

    [Header("Designer settings")]
    [SerializeField] private Optional<float> Despawn = new Optional<float>(4.0f);
    [SerializeField] private Optional<float> Respawn = new Optional<float>(4.0f);
    [SerializeField] private OptionalBlink<float, bool> Blink = new OptionalBlink<float, bool>(2.5f, false);
    [SerializeField] private OptionalGrow<float, float, axis, growth, float> Grow = new OptionalGrow<float, float, axis, growth, float>(1.0f, 5.0f, axis.Z, growth.OneWay, 1.0f);
    //[SerializeField] private OptionalMove<axis, float> Move = new OptionalMove<axis, float>(axis.X, 3.5f);
    [SerializeField] private OptionalMove<float, EndOfPathInstruction, movement> Move = new OptionalMove<float, EndOfPathInstruction, movement>(1.5f, EndOfPathInstruction.Reverse, movement.Automatic);
    [SerializeField] private OptionalRotate<float, axis, rotation> Rotate = new OptionalRotate<float, axis, rotation>(20.0f, axis.Y, rotation.Automatic);


    //PathCreator movePath;
    //public float pathSpeed;
    float pathPoint;
    //public EndOfPathInstruction end;

    Vector3 finalSize;
    Vector3 originalSize;

    [SerializeField] private Transform surface;
    [SerializeField] private PathCreator movePath;

    bool playerOnTop = false;
    
    Transform playerTransform;

    bool growing = false;
    bool moving = false;
    bool rotating = false;

    Vector3 rotationAxis;



    void Awake()
    {
        coll = GetComponent<BoxCollider>();

        //movePath = GetComponentInChildren<PathCreator>();


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

        originalSize = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        if (Rotate.Axis == axis.X)
        {
            rotationAxis = new Vector3(Rotate.Speed, 0, 0);
        }
        if (Rotate.Axis == axis.Y)
        {
            rotationAxis = new Vector3(0, Rotate.Speed, 0);
        }
        if (Rotate.Axis == axis.Z)
        {
            rotationAxis = new Vector3(0, 0, Rotate.Speed);
        }



    }

    private void Start()
    {
        if (Blink.Enabled)
        {
            StartCoroutine(blink());
        }

        if (Grow.Enabled && Grow.Mode == growth.Automatic)
        {
            StartCoroutine(growTo(finalSize));
        }
    }

    private void Update()
    {
        if (playerOnTop)
        {
            if (Despawn.Enabled)
            {
                StartCoroutine(triggerDespawn());
                playerOnTop = false;
            }
        }


        if (Move.Enabled)
        {
            if (Move.Mode == movement.Automatic)
            {
                pathPoint += Move.Speed * Time.deltaTime;
            }

            if (Move.Mode == movement.Reverse)
            {
                if (playerOnTop)
                {
                    pathPoint += Move.Speed * Time.deltaTime;
                }
                else
                {
                    if (pathPoint - Move.Speed * Time.deltaTime > 0)
                    {
                        pathPoint -= Move.Speed * Time.deltaTime;
                    }
                }
            }

            if (Move.Mode == movement.OneWay)
            {
                if (moving)
                {
                    pathPoint += Move.Speed * Time.deltaTime;
                }
            }

            transform.position = new Vector3(movePath.path.GetPointAtDistance(pathPoint, Move.End).x, movePath.path.GetPointAtDistance(pathPoint, Move.End).y, movePath.path.GetPointAtDistance(pathPoint, Move.End).z);

            if (!Rotate.Enabled)
            {
                transform.eulerAngles = new Vector3(0, movePath.path.GetRotationAtDistance(pathPoint, Move.End).eulerAngles.y, 0);
            }
        }

        if (Rotate.Enabled)
        {
            if (Rotate.Mode == rotation.Automatic)
            {
                transform.Rotate(rotationAxis * Time.deltaTime);
            }

            if (Rotate.Mode == rotation.StartOnStand || Rotate.Mode == rotation.TriggerOnStand)
            {
                if (rotating)
                {
                    transform.Rotate(rotationAxis * Time.deltaTime);
                }
            }

        
        }
    }

    private void OnCollisionEnter(Collision collision)
    {


        if (collision.collider.CompareTag("Player"))
        {

            if (collision.collider.transform.position.y >= surface.transform.position.y)
            {
                playerOnTop = true;

                if (!Grow.Enabled)
                {
                    collision.collider.transform.parent = transform;
                    playerTransform = collision.collider.transform;
                }

                /*collision.collider.transform.parent = transform;
                playerTransform = collision.collider.transform;
                Debug.Log(playerTransform);*/

                if (Grow.Enabled && !growing && Grow.Mode != growth.Automatic)
                {
                    StartCoroutine(growTo(finalSize));
                }

                if (Move.Enabled && !moving)
                {
                    moving = true;
                }

                if (Rotate.Enabled)
                {
                    rotating = true;
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
            playerOnTop = false;

            if (Rotate.Enabled && Rotate.Mode == rotation.TriggerOnStand)
            {
                rotating = false;
            }

            if (Grow.Enabled && Grow.Mode == growth.Reverse && !growing)
            {
                StartCoroutine(growTo(originalSize));
            }
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

    IEnumerator growTo(Vector3 newSize)
    {
        float elapsedTime = 0;
        float waitTime = Grow.Time;

        Vector3 currentScale = transform.localScale;

        growing = true;

        while (elapsedTime < waitTime)
        {
            transform.localScale = Vector3.Lerp(currentScale, newSize, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = newSize;
        growing = false;

        if (Grow.Mode == growth.Automatic)
        {
            yield return new WaitForSeconds(Grow.Delay);
            if (newSize == finalSize)
            {
                StartCoroutine(growTo(originalSize));
            }
            if (newSize == originalSize)
            {
                StartCoroutine(growTo(finalSize));
            }
        }

        yield return null;
    }
}