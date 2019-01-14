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
        if (currentTarget != null)
        {
            newDirection = new Vector3(currentTarget.transform.position.x, this.transform.position.y, currentTarget.transform.position.z);
            this.transform.LookAt(newDirection);
        }

        /* Slerp Version
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

        oder so:
        transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( target.transform.position - transform.position ), Time.deltaTime );

        oder:
        Vector3 direction = Point - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.time);
    
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
        if (currentWaypoint < radarWayPoints.Length)
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

    public void GoToNextAvailableWaypoint() //goes to the next available waypoint from startPoint
    {
        GoToWaypoint(NextAvailableWaypoint());
        return;
    }

    public int FirstAvailableWaypoint(int startPoint = 0) //recursive function to find the next available waypoint, finds the first available waypoint by default
    {
        if (startPoint == 666 || startPoint >= radarWayPoints.Length)   //stop condition, check (NextAvailableWaypoint(startPoint) != 666) before setting Waypoint
            return 666;
        if (radarWayPoints[startPoint] != null) //test
            return startPoint;
        else
            return (FirstAvailableWaypoint(startPoint + 1)); //iteration
    }

    public int NextAvailableWaypoint()
    {
        for (int i = 1; i + currentWaypoint < radarWayPoints.Length; i++)
        {
            if (radarWayPoints[currentWaypoint + i] != null) //test
                return (currentWaypoint + i);

            Debug.Log("Did not find waypoint at i = " + i + " with currentWaypoint = " + currentWaypoint + ".");
        }

        Debug.Log("Could not find a next available radar waypoint after currentWaypoint: " + currentWaypoint + ". Defaulting to radar waypoint 0.");
        return 0;
    }

    public void MakeWhite()
    {        
        //image.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
    }
}