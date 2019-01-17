using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalRadarController : MonoBehaviour
{
    Vector3 newDirection;
    private bool isShown;
    private Transform currentTarget;

    private int currentWaypoint;

    public Transform[] radarWayPoints;
    public Image radarImage;

    private LastPlayerSighting lastPlayerSighting;
    private bool isAlarmOn;

    void Start()
    {
        isShown = false;
        currentWaypoint = 0;
        currentTarget = radarWayPoints[currentWaypoint];

        //radarImage = this.GetComponentInChildren<Image>();

        lastPlayerSighting = GameObject.FindGameObjectWithTag("GameController").GetComponent<LastPlayerSighting>();
        isAlarmOn = false;
        
    }

    void Update()
    {
        if (currentTarget != null)
        {
            newDirection = new Vector3(currentTarget.transform.position.x, this.transform.position.y, currentTarget.transform.position.z);
            this.transform.LookAt(newDirection);
        }

        if (isAlarmOn == false && lastPlayerSighting.position != lastPlayerSighting.resetPosition)
        {
            TurnAlarmOn();
        }

        if (isAlarmOn == false && lastPlayerSighting.position == lastPlayerSighting.resetPosition)
        {
            TurnAlarmOff();
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

    public void MakeOrange() ///preserves alpha from radarImage
    {
        var orangeColor = radarImage.color;
        orangeColor.r = 255;
        orangeColor.g = 147;
        orangeColor.b = 0;
        radarImage.color = orangeColor;
    }

    public void MakeRed() ///preserves alpha from radarImage
    {
        var redColor = radarImage.color;
        redColor.r = 255;
        redColor.g = 0;
        redColor.b = 0;
        radarImage.color = redColor;
    }

    public void MakeWhite() ///preserves alpha from radarImage
    {
        var whiteColor = radarImage.color;
        whiteColor.r = 255;
        whiteColor.g = 255;
        whiteColor.b = 255;
        radarImage.color = whiteColor;
    }

    public void SetAlpha(float newAlpha) ///preserves color from radarImage
    {
        var alphaColor = radarImage.color;
        alphaColor.a = newAlpha;
        radarImage.color = alphaColor;
    }

    public void ChangeColor(int newColorIndex, int newColorValue) //Access the r, g, b, a components using [0], [1], [2], [3] respectively.
    {
        var newColor = radarImage.color;
        newColor[newColorIndex] = newColorValue;
        radarImage.color = newColor;
    }

    public void TurnAlarmOn()
    {
        MakeRed();
    }

    public void TurnAlarmOff()
    {
        MakeWhite();
    }
}

