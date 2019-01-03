using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalRadarController : MonoBehaviour
{
    Vector3 newDirection;
    private int x;
    private bool isShown;
    public GameObject target;
    
    void Start()
    {
        isShown = false;        
    }
    
    void Update()
    {
        //    newDirection = new Vector3(target.transform.position.x,
        //                                   this.transform.position.y,
        //                                   target.transform.position.z);


        newDirection = new Vector3(90,
                                       target.transform.position.y,
                                       target.transform.position.z);

        this.transform.LookAt(newDirection);

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

    public void EnableRadarCanas()
    {
        this.GetComponent<Canvas>().enabled = true;
        isShown = true;
    }
}
