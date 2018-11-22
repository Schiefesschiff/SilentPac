using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5;
    public float patrolWaitTime = 1f;
    public Transform[] patrolWayPoints;

    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private LastPlayerSighting lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;


    private void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
    }

    void Update()
    {
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

    }

    void Attacking()
    {
        nav.isStopped = true;
    }

    void Chasing()
    {
        Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

        if (sightingDeltaPos.sqrMagnitude > 4f)
        {
            nav.destination = enemySight.personalLastSighting;
        }

        nav.speed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            lastPlayerSighting.position = lastPlayerSighting.resetPosition;
            enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
            chaseTimer = 0f;
        }
        else
        {
            chaseTimer = 0f;
        }
    }

    void Patrolling()
    {
        nav.speed = patrolSpeed;

        if (nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                if (wayPointIndex == patrolWayPoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }
                patrolTimer = 0;
            }
        }
        else
        {
            patrolTimer = 0f;
        }
        nav.destination = patrolWayPoints[wayPointIndex].position;
    }
}
