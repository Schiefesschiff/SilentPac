using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


public class EnemyAI : MonoBehaviour
{

    [SerializeField] private  float patrolSpeed = 1f;
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float chaseWaitTime = 5;
    [SerializeField] private float patrolWaitTime = 1f;
    [SerializeField] private int SearchingPoints = 3;

    public int health;
    public int maxHeatl = 100;

    public float SearchingRadius = 3;

    public Transform[] patrolWayPoints;

    private float currentSpeed = 1f;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private Animator anim;
    private LastPlayerSighting lastPlayerSighting;
    private GameObject playér;
    private PlayerEnergy playerEnergy;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;
    private int SearchingPointIndex = 0;
    private bool isDeath = false;
    private bool isShocked = false;

    private ThirdPersonCharacter character;

    private void Awake()
    {
        character = GetComponent<ThirdPersonCharacter>();
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        anim = GetComponent<Animator>();

        if (WorldEventController.EnemySpawned != null)
            WorldEventController.EnemySpawned(this);
        health = maxHeatl;
        playerEnergy = player.GetComponent<PlayerEnergy>();

    }

    void Update()
    {
        if (!isDeath && !isShocked )
        {
            nav.enabled = true;

            if (enemySight.playerInSight)
            {
                Attacking();
            }
            else if (enemySight.personalLastSighting != lastPlayerSighting.resetPosition)      
            {
                Chasing();
            }
            else
            {
                Patrolling();
            }


            if (nav.remainingDistance > nav.stoppingDistance)       // distance between enemy / player
            {
                character.Move(nav.desiredVelocity, false, false, currentSpeed);      // for third person controller(animation)
            }
            else
            {
                character.Move(Vector3.zero, false, false, currentSpeed);
            }
        }
        else
        {
            if (isDeath)
            {
                anim.SetBool("DeathTrigger", true);
                character.Move(Vector3.zero, false, false, currentSpeed);
                nav.enabled = false;
            }
        }

        if (isShocked)
        {
            anim.SetBool("isShocked", true);
        }
    }

    public void DeShocked()
    {
        isShocked = false;
        anim.SetBool("isShocked", false);

    }

    public void Shocked()
    {
        anim.SetBool("Attack", false);
        isShocked = true;
        nav.enabled = false;
        character.Move(Vector3.zero, false, false, currentSpeed);
    }

   public void GotDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            //isDeath = true;
        }
    }

    void Attacking()
    {
        nav.speed = chaseSpeed;
        currentSpeed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance && playerEnergy.currentHealth >= 0)
        {
            //print("an dieser stelle attack!!!!!!!!!!!");
            //nav.isStopped = true;
            anim.SetBool("Attack",true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }

        nav.destination = player.transform.position;

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
                if (SearchingPointIndex >= SearchingPoints)     // how mny points for searching
                {
                    lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                    enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
                    chaseTimer = 0f;
                    SearchingPointIndex = 0;
                }
                else
                {
                    SearchingPlayer();
                }
            }
        }
        else
        {
             chaseTimer = 0f;
        }
    }

    void SearchingPlayer()      // random points on NavMeshPlane for navAgent
    {
            //print("´SearchingPlayer Rando Function");
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * SearchingRadius;

            // is random distanz to next radom point


            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                nav.destination = hit.position;
                enemySight.personalLastSighting = hit.position;
                SearchingPointIndex++;
            }
            testPoint = randomPoint;
        
    }

    Vector3 testPoint = new Vector3 (0,0,10);

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(testPoint, 1);
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

    void OnDestroy()
    {
        if (WorldEventController.EnemyRemoved != null)
            WorldEventController.EnemyRemoved(this);// J-D HAUEN
    }
}
