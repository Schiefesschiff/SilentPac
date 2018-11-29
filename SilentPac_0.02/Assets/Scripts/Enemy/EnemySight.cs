using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;        // position from hearing

    private NavMeshAgent nav;
    private SphereCollider col;
    private Animator anim;
    private LastPlayerSighting lastPlayerSighting;
    private GameObject player;
    private Animator playerAnim;

    private Vector3 previousSighting;
       
    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();
        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerAnim = player.GetComponent<Animator>();

        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    private void Update()
    {
        if (lastPlayerSighting.position != previousSighting)
        {
            previousSighting = lastPlayerSighting.position;
        }

        //previousSighting = lastPlayerSighting.position;

        //if (playerHealth.health > 0)                  // player is death?
        //{
        //    anim.SetBool(INsightBool, playerInSight);
        //}
        //else
        //{
        //    anim.SetBool(INsightBool, false);    << dann lauf weiter lauf weiter
        //}
    }

        void OnTriggerStay(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInSight = false;
                Vector3 direction = other.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);

                if (angle < fieldOfViewAngle * 0.5f)
                {
                    RaycastHit hit;

                    // raycast +1hight (transform.up) and collder radius
                    if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius))
                    {
                        if (hit.collider.gameObject == player)
                        {
                            playerInSight = true;
                            lastPlayerSighting.position = player.transform.position; // alarm 
                        }
                    }

                }

                // todo player animator in bewegung?
                // todo player is shoot?
                if (playerAnim.GetBool("Sprint"))       //ToDO  jump , Walking
                {
                    if (CalculatePathLength(player.transform.position) <= col.radius)       // player inside sphereCollider?
                    {
                        personalLastSighting = player.transform.position;
                    }
                }

            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                playerInSight = false;
            }
        }

        float CalculatePathLength(Vector3 targetPosition)
        {
            NavMeshPath path = new NavMeshPath();

            if (nav.enabled)
            {
                nav.CalculatePath(targetPosition, path);        // new path to target
            }

            Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];      // +2 for target and this
            allWayPoints[0] = transform.position;
            allWayPoints[allWayPoints.Length - 1] = targetPosition;

            for (int i = 0; i < path.corners.Length; i++)
            {
                allWayPoints[i + 1] = path.corners[i];
            }

            float pathLength = 0f;

            for (int i = 0; i < allWayPoints.Length-1 ; i++)
            {
                pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
            }

            return pathLength;
        }

    
}
