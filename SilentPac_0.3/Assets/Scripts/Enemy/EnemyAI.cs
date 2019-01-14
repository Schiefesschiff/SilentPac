using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask Enemy2Mask;
    [SerializeField] private bool Enemy2 = false;
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseWaitTime = 5;
    [SerializeField] private float patrolWaitTime = 1f;
    [SerializeField] private float searchWaitTime = 1f;
    [SerializeField] private int SearchingPoints = 3;

    public Transform[] patrolWayPoints;

    private ThirdPersonCharacter character;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private Animator anim;
    private LastPlayerSighting lastPlayerSighting;
    private PlayerEnergy playerEnergy;      // ersetzen mit world position = resetposition
    private EnemyHealth enemyHealth;


    private float chaseTimer;   // timer for "wait time" after chasing
    private float patrolTimer;  // timer for waittime on patrol point
    private float searchTimer;
    private int wayPointIndex;
    private int SearchingPointIndex = 0;
    [HideInInspector]public bool isDeath = false;
    private bool isShocked = false;
    private float currentSpeed = 1f;
    public bool isSearch = false;



    private enum EnemyStatus
    {
        Attacking, Chasing, Patrolling, Death, Shocked, Searching
    }

    [SerializeField]
    private EnemyStatus enemyStatus = EnemyStatus.Patrolling;

    private void Awake()
    {
        character = GetComponent<ThirdPersonCharacter>();
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        anim = GetComponent<Animator>();
        playerEnergy = player.GetComponent<PlayerEnergy>();
        enemyHealth = GetComponent<EnemyHealth>();
        nav.updateRotation = false;
    }

    private void Update()
    {
        anim.SetBool("Attack", false);

        if (enemySight.playerInSight && playerEnergy.currentHealth > 0)
        {
            enemyStatus = EnemyStatus.Attacking;
        }
        else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition)
        {
            enemyStatus = EnemyStatus.Chasing;
        }
        else if (enemySight.personalLastSighting == lastPlayerSighting.resetPosition)
        {
            enemyStatus = EnemyStatus.Patrolling;
        }
        
        if (isSearch && lastPlayerSighting.position == lastPlayerSighting.resetPosition && enemyStatus != EnemyStatus.Chasing)
        {
            enemyStatus = EnemyStatus.Searching;
        }
        else if (isSearch && lastPlayerSighting.position != lastPlayerSighting.resetPosition)
        {
            isSearch = false;
        }

        if (isShocked)
        {
            enemyStatus = EnemyStatus.Shocked;
        }

        if (isDeath)
        {
            enemyStatus = EnemyStatus.Death;
        }


        switch (enemyStatus)
        {
            case EnemyStatus.Patrolling:
                Patrolling();
                //print("patrolling switch");
                break;
            case EnemyStatus.Chasing:
                Chasing();
                //print("chasing switch");
                break;
            case EnemyStatus.Searching:
                SearchingPlayer();
                //print("searching switch");
                break;
            case EnemyStatus.Attacking:
                Attacking();
                //print("attacking switch");
                break;
            case EnemyStatus.Shocked:
                Shocked();
                //print("shocked switch");
                break;
            case EnemyStatus.Death:
                Dying();
                //print("Death switch");
                break;
        }

        if (nav.remainingDistance > nav.stoppingDistance)       // distance between enemy / player
        {
            character.Move(nav.desiredVelocity, false, false, currentSpeed);      // for third person controller(animation)
            //print(nav.desiredVelocity);
        }
        else
        {
            character.Move(Vector3.zero, false, false, currentSpeed);      // for third person controller(animation)
        }
    }

    void Dying()
    {
        anim.SetBool("DeathTrigger", true);
    }

    private float waitTimer = 0;
    private int waitTime = 10;
    private int indexRotations = 3;
    private int rotations = 0;
    

    private bool WaitRotation()
    {
        float rot = 1;
        waitTimer += Time.deltaTime;

        if (waitTimer > waitTime)
        {
            rot = Random.Range(-1,1);
            rotations++;
            waitTimer = 0;
            if (rotations > indexRotations)
            {
                rotations = 0;
                rot = 0;
                return true;
            }
        }
        transform.rotation = Quaternion.Euler(0, rot * 3f, 0) * transform.rotation;
        anim.SetFloat("Turn", rot);
        return false;
    }

    void Patrolling()
    {
        nav.speed = patrolSpeed;
        currentSpeed = patrolSpeed;

        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            //patrolTimer += Time.deltaTime;
            //if (patrolTimer >= patrolWaitTime)
            //{
            if (wayPointIndex == patrolWayPoints.Length - 1)
            {
                wayPointIndex = 0;
            }
            else
            {
                wayPointIndex++;
            }
            //patrolTimer = 0;
            //}
        }
        else
        {
            //patrolTimer = 0f;
        }
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }

    void Chasing()
    {
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            if (!Enemy2)
            {
                nav.destination = enemySight.personalLastSighting;
            }
            else
            {
                if (enemySight.personalLastSighting != lastPlayerSighting.position)
                {
                    RaycastHit hit2;
                    if (Physics.Raycast(player.position + player.up, player.forward, out hit2, Mathf.Infinity, Enemy2Mask))
                    {
                        nav.destination = (hit2.transform.position + player.transform.position) / 2;
                        testPoint = (hit2.transform.position + player.transform.position) / 2;      // for gizmo
                    }
                }
            }


        }

        nav.speed = chaseSpeed;
        currentSpeed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            //if (WaitRotation())
            //{
            if (chaseTimer > chaseWaitTime)
            {
                    lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                    enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                    chaseTimer = 0f;
                    isSearch = true;
             }
        //}
    }
        else
        {
            chaseTimer = 0f;
        }
    }

    private float currentDist = 100;
    public Vector3 currentSearchPoint;

    void SearchingPlayer()      
    {
        nav.speed = chaseSpeed;
        currentSpeed = chaseSpeed;
        // todo nächste weg gabelung suchen
        // in richtung des spielers
        if (nav.remainingDistance < nav.stoppingDistance)
        {
            if (SearchingPointIndex < SearchingPoints)     // all points searched?
            {
                searchTimer += Time.deltaTime;
                if (searchTimer > searchWaitTime)
                {
                    for (int i = 0; i < lastPlayerSighting.forks.Length; i++)
                    {
                        float dist = Vector3.Distance(transform.position, lastPlayerSighting.forks[i].transform.position);

                        if (dist < currentDist && dist > 10)
                        {
                            currentDist = dist;
                            //print(currentDist);
                            currentSearchPoint = lastPlayerSighting.forks[i].transform.position;
                        }
                    }
                    nav.SetDestination(currentSearchPoint);
                    currentDist = 100;
                    searchTimer = 0;
                    SearchingPointIndex++;
                }
            }
            else
            {
                isSearch = false;
                SearchingPointIndex = 0;
            }



        }

    }

    Vector3 testPoint = new Vector3(0, 0, 10);

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(testPoint, 0.5f);
    }

    void Attacking()
    {
        nav.speed = chaseSpeed;
        currentSpeed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }

        nav.destination = player.transform.position;

    }

    public void ShockedAnimationEvent()
    {
        isShocked = true;
    }

    public void DeShockedAnimationEvent()     // from animationEvent?
    {
        isShocked = false;
        anim.SetBool("isShocked", false);
    }

    void Shocked()
    {
        anim.SetBool("isShocked", true);
    }

}
