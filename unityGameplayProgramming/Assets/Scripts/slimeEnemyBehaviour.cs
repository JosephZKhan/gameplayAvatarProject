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
    public float attackRadius = 7.0f;

    Rigidbody rb;

    bool canAttack = true;
    bool damageActive = false;

    [SerializeField] playerController2 playerScriptRef;
    [SerializeField] BoxCollider playerPunch;

    public int knockback = 150;

    [SerializeField] GameObject prefab;

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
            canAttack = true;
            damageActive = false;
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
            canAttack = true;
            damageActive = false;
            if (playerRef != null)
            {
                agent.SetDestination(playerRef.transform.position);
                //Debug.Log(agent.remainingDistance);
                if (agent.remainingDistance <= attackRadius)
                {
                    currentStatus = status.Attack;
                }
            }
        }

        if (currentStatus == status.Attack)
        {
            //Debug.Log("in attack mode");
            agent.SetDestination(playerRef.transform.position);
            agent.isStopped = true;
            if (!canAttack)
            {
                transform.LookAt(playerRef.transform);
            }
            if (agent.remainingDistance >= attackRadius)
            {
                currentStatus = status.Chase;
                agent.isStopped = false;
            }
            if (canAttack)
            {
                canAttack = false;
                StartCoroutine(attack());
            }
        }

       

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            playerRef = other.gameObject;
            StartCoroutine(detectPlayer());
        }

        if (other == playerPunch)
        {
            playerScriptRef.triggerPunchEffect();
            
            /*GameObject newSlime1 = Instantiate(prefab, transform.localPosition + transform.right, Quaternion.identity) as GameObject;
            GameObject newSlime2 = Instantiate(prefab, transform.localPosition - transform.right, Quaternion.identity) as GameObject;
            newSlime1.transform.localPosition = transform.localScale / 2;
            newSlime2.transform.localPosition = transform.localScale / 2;*/


            Destroy(gameObject);
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
            //Debug.Log("touch player");
            if (damageActive)
            {
                playerScriptRef.takeDamage(1, transform, knockback);
            }
        }
    }

    IEnumerator detectPlayer()
    {
        yield return new WaitForSeconds(0.1f);

        Vector3 targetDir = (playerRef.transform.position - transform.position);
        targetDir.Normalize();

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

    IEnumerator attack()
    {
        canAttack = false;
        Debug.Log("Wait");
        yield return new WaitForSeconds(1.0f);

        Debug.Log("Launch");
        //transform.Translate(transform.forward * Time.deltaTime * 300.0f, Space.World);
        StartCoroutine(moveToPoint(transform.position + (transform.forward * 10.0f)));
        damageActive = true;
        yield return new WaitForSeconds(2.0f);

        Debug.Log("Stop");
        damageActive = false;
        canAttack = true;

    }

    IEnumerator moveToPoint(Vector3 endPos)
    {
        float elapsedTime = 0;
        float waitTime = 1.0f;

        Vector3 currentPosition = new Vector3();
        currentPosition = transform.position;

        while (elapsedTime < waitTime)
        {
            transform.position = Vector3.Lerp(currentPosition, endPos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = endPos;
        yield return null;
    }
}
