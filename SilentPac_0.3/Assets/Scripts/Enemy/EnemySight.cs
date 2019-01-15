using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    public float fieldOfViewAngle = 110f;
    public bool playerInSight;
    public Vector3 personalLastSighting;        // position from hearing

    [SerializeField] private LayerMask walls;
    private PlayerEnergy playerEnergy;
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
        playerEnergy = player.GetComponent<PlayerEnergy>();
        playerAnim = player.GetComponent<Animator>();

        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    public float testangle;

    private void Update()
    {
        if (lastPlayerSighting.position != previousSighting)
        {
            personalLastSighting = lastPlayerSighting.position;
        }

        previousSighting = lastPlayerSighting.position;

        //ShowSearchRadius();

        if (playerEnergy.currentHealth <= 0)                  // player is death?
        {
            personalLastSighting = lastPlayerSighting.resetPosition;
        }
    }


    public Transform winkel1;
    public Transform winkel2;

    public void ShowSearchRadius()
    {

        Debug.DrawLine(transform.position, winkel1.position, Color.blue);

        Debug.DrawLine(transform.position, winkel2.position, Color.blue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.GetComponent<PlayerController>().enemiesClose.Add(this.transform);
        }
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
                //Debug.DrawLine(transform.position + transform.up, player.transform.position + transform.up, Color.red);

                // raycast +1hight (transform.up) and collder radius
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, col.radius, walls))
                {
                    Debug.DrawLine(transform.position + transform.up, hit.point, Color.red);

                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerSighting.position = player.transform.position; // alarm 
                    }
                }
            }
            else if (Vector3.Distance(transform.position, player.transform.position) <= 2)
            {
                playerInSight = true;
                lastPlayerSighting.position = player.transform.position; // alarm 
            }

            // todo player animator in bewegung?
            // tdo player is shoot?
            if (playerAnim.GetBool("Run"))       // is Player Run and path length < colliderRadius =  Chasing at last seeing point
            {
                if (CalculatePathLength(player.transform.position) <= col.radius)       // 
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
               player.transform.GetComponent<PlayerController>().enemiesClose.Remove(this.transform);
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
