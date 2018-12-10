using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;  //  for array query  Contains



public class EnemyScript : MonoBehaviour
{
    private bool isPLayerDetected = false;
    public GameObject player;
    private NavMeshAgent navMeshAgent;
    public GameObject WayPointsObj;
    public Transform[] wayPointsObj;
    private Transform currentWayPointObj;


    private int wayPointCount; 

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        AddWayPointArray();                                          // added to WayPoint Array
        navMeshAgent.SetDestination(wayPointsObj[0].position);      // first Waypoint
        currentWayPointObj = wayPointsObj[0];
    }

    void AddWayPointArray()  // added WayPoints to Array
    {
        int i = WayPointsObj.GetComponentInChildren<Transform>().childCount;        // How many WayPoints
        wayPointsObj = WayPointsObj.GetComponentsInChildren<Transform>();           // Search and Added To array

        wayPointsObj = new Transform[i];    // create a new Length
        for (int j = 0; j < i; j++)         // added to new array
        {
            wayPointsObj[j] = WayPointsObj.GetComponentInChildren<Transform>().GetChild(j);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (isPLayerDetected == false)
        {
            if (col.tag == "WayPoint")
            {
                if (wayPointsObj.Contains(col.transform))       // is waypoint in the waypoint_Array
                {
                    navMeshAgent.SetDestination(NextPosition());        // next waypoint
                }
            }
        }

    }

    Vector3 NextPosition()
    {
        wayPointCount++;
        print("Waypointcount " + wayPointCount);
        if (wayPointCount == wayPointsObj.Length)       
        {
            print("if schleife waypoints");
            wayPointCount = 0;
        }

        return wayPointsObj[wayPointCount].transform.position;
    }

    public void NewTarget()
    {
        print("newTarget");
        isPLayerDetected = true;
        navMeshAgent.SetDestination(player.transform.position);
    }

}
