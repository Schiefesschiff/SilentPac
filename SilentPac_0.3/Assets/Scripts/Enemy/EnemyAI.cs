using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


public class EnemyAI : MonoBehaviour
{
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
    public int SearchingPointIndex = 0;
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

        if (isSearch && lastPlayerSighting.position == lastPlayerSighting.resetPosition)
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
            nav.destination = enemySight.personalLastSighting;
        }

        nav.speed = chaseSpeed;
        currentSpeed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer > chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0f;
                isSearch = true;
            }
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
        // todo nächste weg gabelung suchen
        // in richtung des spielers
        if (nav.remainingDistance < nav.stoppingDistance)
        {
            searchTimer += Time.deltaTime;

            if (SearchingPointIndex < SearchingPoints)     // all points searched?
            {
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
        Gizmos.DrawSphere(testPoint, 1);
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

//{

//    [SerializeField] private  float patrolSpeed = 1f;
//    [SerializeField] private float chaseSpeed = 2f;
//    [SerializeField] private float chaseWaitTime = 5;
//    [SerializeField] private float patrolWaitTime = 1f;
//    [SerializeField] private int SearchingPoints = 3;

//    public int health;
//    public int maxHeatl = 100;

//    public float SearchingRadius = 3;

//    public Transform[] patrolWayPoints;

//    private float currentSpeed = 1f;
//    private EnemySight enemySight;
//    private NavMeshAgent nav;
//    private Transform player;
//    private Animator anim;
//    private LastPlayerSighting lastPlayerSighting;
//    private PlayerEnergy playerEnergy;
//    private float chaseTimer;
//    private float patrolTimer;
//    private int wayPointIndex;
//    private int SearchingPointIndex = 0;
//    private bool isDeath = false;
//    private bool isShocked = false;

//    private ThirdPersonCharacter character;

//    private void Awake()
//    {
//        character = GetComponent<ThirdPersonCharacter>();
//        enemySight = GetComponent<EnemySight>();
//        nav = GetComponent<NavMeshAgent>();
//        player = GameObject.FindGameObjectWithTag("Player").transform;
//        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
//        anim = GetComponent<Animator>();

//        if (WorldEventController.EnemySpawned != null)
//            WorldEventController.EnemySpawned(this);

//        health = maxHeatl;
//        playerEnergy = player.GetComponent<PlayerEnergy>();

//    }

//    void Update()
//    {
//        if (!isDeath && !isShocked )
//        {
//            nav.enabled = true;

//            if (enemySight.playerInSight)
//            {
//                Attacking();
//            }
//            else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition)      
//            {
//                Chasing();
//            }
//            else
//            {
//                Patrolling();
//            }


//            if (nav.remainingDistance > nav.stoppingDistance)       // distance between enemy / player
//            {
//                character.Move(nav.desiredVelocity, false, false, currentSpeed);      // for third person controller(animation)
//            }
//            else
//            {
//                character.Move(Vector3.zero, false, false, currentSpeed);
//            }
//        }
//        else
//        {
//            if (isDeath)
//            {
//                anim.SetBool("DeathTrigger", true);
//                character.Move(Vector3.zero, false, false, currentSpeed);
//                nav.enabled = false;
//            }
//        }

//        if (isShocked)
//        {
//            anim.SetBool("isShocked", true);
//        }
//    }

//    public void DeShocked()
//    {
//        isShocked = false;
//        anim.SetBool("isShocked", false);

//    }

//    public void Shocked()
//    {
//        anim.SetBool("Attack", false);
//        isShocked = true;
//        nav.enabled = false;
//        character.Move(Vector3.zero, false, false, currentSpeed);
//    }

//   public void GotDamage(int damage)
//    {
//        health -= damage;
//        if (health <= 0)
//        {
//            //isDeath = true;
//        }
//    }

//    void Attacking()
//    {
//        nav.speed = chaseSpeed;
//        currentSpeed = chaseSpeed;

//        if (nav.remainingDistance < nav.stoppingDistance && playerEnergy.currentHealth >= 0)
//        {
//            //print("an dieser stelle attack!!!!!!!!!!!");
//            //nav.isStopped = true;
//            anim.SetBool("Attack",true);
//        }
//        else
//        {
//            anim.SetBool("Attack", false);
//        }

//        nav.destination = player.transform.position;

//    }

//    void Chasing()
//    {
//        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

//        if (sightingDeltaPos.sqrMagnitude > 4f)
//        {
//            nav.destination = enemySight.personalLastSighting;
//        }

//        nav.speed = chaseSpeed;
//        currentSpeed = chaseSpeed;

//        if (nav.remainingDistance < nav.stoppingDistance)
//        {
//            chaseTimer += Time.deltaTime;

//            if (chaseTimer > chaseWaitTime)
//            {
//                if (SearchingPointIndex >= SearchingPoints)     // how mny points for searching
//                {
//                    lastPlayerSighting.position = lastPlayerSighting.resetPosition;
//                    enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
//                    chaseTimer = 0f;
//                    SearchingPointIndex = 0;
//                }
//                else
//                {
//                    SearchingPlayer();
//                }
//            }
//        }
//        else
//        {
//             chaseTimer = 0f;
//        }
//    }

//    void SearchingPlayer()      // random points on NavMeshPlane for navAgent
//    {
//            //print("´SearchingPlayer Rando Function");
//            Vector3 randomPoint = transform.position + Random.insideUnitSphere * SearchingRadius;

//            // is random distanz to next radom point


//            NavMeshHit hit;
//            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
//            {
//                nav.destination = hit.position;
//                enemySight.personalLastSighting = hit.position;
//                SearchingPointIndex++;
//            }
//            testPoint = randomPoint;

//    }

//    Vector3 testPoint = new Vector3 (0,0,10);

//    void OnDrawGizmosSelected()
//    {
//        // Draw a yellow sphere at the transform's position
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawSphere(testPoint, 1);
//    }

//    void Patrolling()
//    {
//        nav.speed = patrolSpeed;
//        currentSpeed = patrolSpeed;

//        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
//        {
//            //patrolTimer += Time.deltaTime;
//            //if (patrolTimer >= patrolWaitTime)
//            //{
//                if (wayPointIndex == patrolWayPoints.Length - 1)
//                {
//                    wayPointIndex = 0;
//                }
//                else
//                {
//                    wayPointIndex++;
//                }
//                //patrolTimer = 0;
//            //}
//        }
//        else
//        {
//            //patrolTimer = 0f;
//        }
//        nav.destination = patrolWayPoints[wayPointIndex].position;
//    }

//    void OnDestroy()
//    {
//        if (WorldEventController.EnemyRemoved != null)
//            WorldEventController.EnemyRemoved(this);// J-D HAUEN
//    }
//}
