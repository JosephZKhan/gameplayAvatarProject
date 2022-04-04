using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class slimeEnemyBehaviour : MonoBehaviour
{

    NavMeshAgent agent;
    GameObject playerRef;
    Transform pointHolder;
    Vector3[] patrolPoints;
    int patrolPointIdx = 0;
    int patrolPointMax;


    public enum status { Patrol, Chase, Attack, Hurt };
    public status currentStatus = status.Patrol;


    public float chaseRadius = 10.0f;
    public float attackRadius = 4.0f;

    Rigidbody rb;

    bool canAttack = true;
    bool damageActive = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        pointHolder = transform.GetChild(0);

        patrolPoints = new Vector3[pointHolder.childCount];
        for (int i = 0; i < pointHolder.childCount; i++)
        {
            patrolPoints[i] = (pointHolder.GetChild(i).transform.position);
        }
        patrolPointMax = pointHolder.childCount;

        //rb = gameObject.GetComponent<Rigidbody>();


    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentStatus == status.Patrol)
        {
            agent.SetDestination(patrolPoints[patrolPointIdx]);
            if (transform.position.x == patrolPoints[patrolPointIdx].x && transform.position.z == patrolPoints[patrolPointIdx].z)
            {
                patrolPointIdx++;
                if (patrolPointIdx >= patrolPointMax)
                {
                    patrolPointIdx = 0;
                }
            }
        }

        if (currentStatus == status.Chase)
        {
            if (playerRef != null)
            {
                agent.SetDestination(playerRef.transform.position);
            }
        }

        if (currentStatus == status.Attack)
        {
            Debug.Log("in attack mode");
        }

       

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            playerRef = other.gameObject;
            StartCoroutine(detectPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player out of range.");
            currentStatus = status.Patrol;
            playerRef = null;
            StopAllCoroutines();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (damageActive)
            {
                Debug.Log("hurt player.");
            }
        }
    }

    IEnumerator detectPlayer()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 targetDir = (playerRef.transform.position - transform.position);
        targetDir.Normalize();
        if (Physics.Raycast(transform.position, targetDir, attackRadius))
        {
            if (currentStatus == status.Chase)
            {
                Debug.Log("enter attack mode!");
                currentStatus = status.Attack;
                yield return null;
            }
            
        }
        if (Physics.Raycast(transform.position, targetDir, chaseRadius))
        {
            if (currentStatus == status.Patrol)
            {
                Debug.Log("player detected!");
                currentStatus = status.Chase;
                yield return null;
            }
            
        }
        
    }

    /*IEnumerator attack()
    {
        yield return new WaitForSeconds(1.0f);

        rb.AddForce(transform.forward * 5.0f);
        damageActive = true;

        yield return new WaitForSeconds(1.0f);
        damageActive = false;

    }*/
}
