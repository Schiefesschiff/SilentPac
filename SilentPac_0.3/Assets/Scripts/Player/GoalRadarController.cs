using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalRadarController : MonoBehaviour
{
    Vector3 newDirection;
    private bool isShown;
    public Transform currentTarget;

    private int currentWaypoint;

    public Transform[] radarWayPoints;

    void Start()
    {
        isShown = false;
        currentWaypoint = 0;
    }
    
    void Update()
    {
        //    newDirection = new Vector3(target.transform.position.x,
        //                                   this.transform.position.y,
        //                                   target.transform.position.z);

        //newDirection = new Vector3(0f,
        //                               currentTarget.transform.position.y,
        //                               0f);
        if (currentTarget != null)
        {
            newDirection = new Vector3(currentTarget.transform.position.x,
                                           currentTarget.transform.position.y,
                                           currentTarget.transform.position.z);

            this.transform.LookAt(newDirection);
        }
        
        /* Slerp Version
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
        */
    }

    public void DisableRadarCanvas()
    {
        this.GetComponent<Canvas>().enabled = false;
        isShown = false;
    }

    public void EnableRadarCanvas()
    {
        this.GetComponent<Canvas>().enabled = true;
        isShown = true;
    }

    public void GoToWaypoint(int waypointIndex)
    {
        currentTarget = radarWayPoints[waypointIndex];
    }

    public void GoToOverride(Transform overrideWaypoint) //goes to a waypoint from outside the array
    {
        currentTarget = overrideWaypoint;
        EnableRadarCanvas();
    }

    public void GoToNextWaypoint() //goes to the next waypoint, otherwise disables RadarCanvas
    {
        if (currentWaypoint < radarWayPoints.Length - 1)
        {
            if (!(radarWayPoints[currentWaypoint + 1] == null))
                GoToWaypoint(++currentWaypoint);
            else
                DisableRadarCanvas();
        }
        else
        {
            DisableRadarCanvas();
            Debug.Log("All waypoints reached. Disabled RadarCanvas.");
        }
    }

    public void GoToNextAvailableWaypoint(int startPoint = 666) //goes to the next available waypoint from startPoint
    {
        if (startPoint == 666)            //defaults startPoint to currentWaypoint
            startPoint = currentWaypoint;

        int nextWaypoint = NextAvailableWaypoint(startPoint);

        if (nextWaypoint == 666)
            return;
        else
            GoToWaypoint(nextWaypoint);

        return;
    }

    public int NextAvailableWaypoint(int startPoint = 0) //recursive function to find the next available waypoint, finds the first available waypoint by default
    {
        if (startPoint == 666 || startPoint >= radarWayPoints.Length)   //stop condition, check (NextAvailableWaypoint(startPoint) != 666) before setting Waypoint
            return 666;
        if (radarWayPoints[startPoint] != null) //test
            return startPoint;
        else
            return (NextAvailableWaypoint(startPoint + 1)); //iteration
    }
}