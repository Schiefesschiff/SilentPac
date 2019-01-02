using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float smooth = 1.5f;

    private Transform player;

    private Vector3 relCameraPos;       // direction player
    private float relCameraPosMag;      // Distance to Player
    private Vector3 newPos;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;
    }

    private void FixedUpdate()
    {
        Vector3 standartPos = player.position + relCameraPos;
        Vector3 abovePos = player.position + Vector3.up * relCameraPosMag;
        Vector3[] checkpoints = new Vector3[5];
        checkpoints[0] = standartPos;
        checkpoints[1] = Vector3.Lerp(standartPos, abovePos, 0.25f);
        checkpoints[2] = Vector3.Lerp(standartPos, abovePos, 0.50f);
        checkpoints[3] = Vector3.Lerp(standartPos, abovePos, 0.75f);
        checkpoints[4] = abovePos;

        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (ViewPosCheck(checkpoints[i])) 
            {
                break;
            }
        }
        transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime); // move new Position
        SmoothLookAt();

    }

    bool ViewPosCheck(Vector3 checkPos)     // hit raycast player? 
    {
        RaycastHit hit;

        if (Physics.Raycast(checkPos, player.position - checkPos, out hit, relCameraPosMag))
        {
            if (hit.transform != player)
            {
                return false;  
            }
        }
        newPos = checkPos;      // new Camera Position
        return true;
    }

    void SmoothLookAt()
    {
        Vector3 relPlayerPosition = player.position - transform.position;
        Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, smooth * Time.deltaTime);
    }
}
