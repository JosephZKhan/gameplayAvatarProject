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


    bool canAttack = true;
    bool damageActive = false;

    playerController2 playerScriptRef;
    BoxCollider playerPunch;

    public int knockback = 50;

    [SerializeField] slimeEnemyBehaviour prefab;
    [SerializeField] GameObject ringPrefab;

    public int health = 3;

    [SerializeField] Material[] materials;

    public float launchSpeed = 10.0f;
    Renderer rend;




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

        playerScriptRef = GameObject.FindWithTag("Player").GetComponent<playerController2>();
        playerPunch = GameObject.FindWithTag("Player").GetComponent<BoxCollider>();

        rend = GetComponent<Renderer>();
        rend.sharedMaterial = materials[0];

    }
    void FixedUpdate()
    {
        if (currentStatus == status.Patrol)
        {
            rend.sharedMaterial = materials[0];

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
            rend.sharedMaterial = materials[1];

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
            if (playerRef != null)
            {
                agent.SetDestination(playerRef.transform.position);
            }
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
                
                StopAllCoroutines();
                StartCoroutine(attack());
            }
        }

        if (transform.localScale.x < 1.0f)
        {
            Destroy(gameObject);
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
            playerScriptRef.endPunch();
            takeDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("player out of range.");
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
                //Debug.Log("player detected!");
                currentStatus = status.Chase;
                yield return null;
            }
            
        }
        
    }

    IEnumerator attack()
    {
        canAttack = false;
        //Debug.Log("Wait");
        yield return new WaitForSeconds(1.0f);
        rend.sharedMaterial = materials[2];

        //Debug.Log("Launch");
        //transform.Translate(transform.forward * Time.deltaTime * 300.0f, Space.World);
        StartCoroutine(moveToPoint(transform.position + (transform.forward * 10.0f), 1.0f));
        damageActive = true;
        yield return new WaitForSeconds(2.0f);

        //Debug.Log("Stop");
        damageActive = false;
        canAttack = true;
        rend.sharedMaterial = materials[1];

    }

    IEnumerator moveToPoint(Vector3 endPos, float waitTime)
    {
        float elapsedTime = 0;

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

    void takeDamage()
    {
        health--;
        if (playerScriptRef.hasSuperPunch)
        {
            health = 0;
        }
        if (health > 0)
        {
            float knockback = 7.0f;
            Vector3 movePos = new Vector3(transform.position.x + playerRef.transform.forward.x * knockback, transform.position.y, transform.position.z + playerRef.transform.forward.z * knockback);
            
            StartCoroutine(moveToPoint(movePos, 0.3f));
        }
        else
        {
            if (transform.localScale.x > 1.0f)
            {
                slimeEnemyBehaviour newSlime1 = Instantiate(prefab, transform.position + (transform.right * 3), Quaternion.identity);
                slimeEnemyBehaviour newSlime2 = Instantiate(prefab, transform.position - (transform.right * 3), Quaternion.identity);
                newSlime1.transform.localScale = transform.localScale / 2;
                newSlime2.transform.localScale = transform.localScale / 2;
                if (newSlime1.transform.localScale.x == 2)
                {
                    newSlime1.health = 2;
                    newSlime2.health = 2;
                }
                if (newSlime1.transform.localScale.x == 1)
                {
                    newSlime1.health = 1;
                    newSlime2.health = 1;
                }    
            }
            else
            {
                GameObject ring1 = Instantiate(ringPrefab, transform.position + (transform.right * 2), Quaternion.identity) as GameObject;
                GameObject ring2 = Instantiate(ringPrefab, transform.position - (transform.right * 2), Quaternion.identity) as GameObject;
            }
            Destroy(gameObject);
        }
    }



}
