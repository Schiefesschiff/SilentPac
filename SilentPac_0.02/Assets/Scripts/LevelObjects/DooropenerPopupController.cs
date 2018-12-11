using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooropenerPopupController : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private GameObject player;

    public bool isShown = true;
        
    void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        /*
        door = GameObject.FindGameObjectWithTag("Door");
        doorController = door.GetComponent<DoorController>();

        showTooltip = true;
        isDoorOpen = false;

        hasEnergy = false;
        */
    }

    void LateUpdate()
    {
        if (isShown)
        {
            RotateToCamera();
        }
        /*
        if ()
        {

        }
        */
    }

    void RotateToCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
        //Transform.EulerAngles
    }
}
