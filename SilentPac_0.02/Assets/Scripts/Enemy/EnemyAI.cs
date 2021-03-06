﻿using System.Collections;
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

    public float SearchingRadius = 3;

    public Transform[] patrolWayPoints;

    private float currentSpeed = 1f;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Transform player;
    private Animator anim;
    private LastPlayerSighting lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;
    private int wayPointIndex;
    private int SearchingPointIndex = 0;


    private ThirdPersonCharacter character;

    private void Awake()
    {
        character = GetComponent<ThirdPersonCharacter>();
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        anim = GetComponent<Animator>();
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


        if (nav.remainingDistance > nav.stoppingDistance)       // distance between enemy / player
        {
            character.Move(nav.desiredVelocity, false, false, currentSpeed);      // for third person controller(animation)
        }
        else
        {
            character.Move(Vector3.zero, false, false, currentSpeed);
        }
    }


    void Attacking()
    {
        nav.speed = chaseSpeed;
        currentSpeed = chaseSpeed;

        if (nav.remainingDistance < nav.stoppingDistance)
        {
            print("an dieser stelle attack!!!!!!!!!!!");
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
            print("´SearchingPlayer Rando Function");
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
}
