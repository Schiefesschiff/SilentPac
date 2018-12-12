using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooropenerPopupController : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private GameObject player;
    private DooropenerInteraction dooropenerInteraction;
    public GameObject dooropener;

    public bool isShown = true;

    public GameObject Panel0_UseKey;
    public GameObject Panel1_NeedKey;
    public GameObject Panel2_NeedEnergy;
    public GameObject Panel3_DoorOpened;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();

        dooropenerInteraction = dooropener.GetComponent<DooropenerInteraction>();

        //this.GetComponent<>();

        /*
        door = GameObject.FindGameObjectWithTag("Door");
        doorController = door.GetComponent<DoorController>();

        showTooltip = true;
        isDoorOpen = false;

        hasEnergy = false;
        */

        DisableCanvas();
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

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            EnableCanvas();
            ChangePopup(WhichPopup());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            DisableCanvas();
    }

    void RotateToCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
        //Transform.EulerAngles
    }
    
    public int WhichPopup()
    {
        if (dooropenerInteraction.isDoorOpen)
            return 3;
        else if (!dooropenerInteraction.hasEnergy)
            return 2;
        else if (!playerInventory.hasKey)
            return 1;
        else if (playerInventory.hasKey)
            return 0;
        
        return 0;
    }

    public void ChangePopup(int index)
    {
        switch (index)
        {
            case 0:
                Panel0_UseKey.gameObject.SetActive(true);
                Panel1_NeedKey.gameObject.SetActive(false);
                Panel2_NeedEnergy.gameObject.SetActive(false);
                Panel3_DoorOpened.gameObject.SetActive(false);
                break;
            case 1:
                Panel0_UseKey.gameObject.SetActive(false);
                Panel1_NeedKey.gameObject.SetActive(true);
                Panel2_NeedEnergy.gameObject.SetActive(false);
                Panel3_DoorOpened.gameObject.SetActive(false);
                break;
            case 2:
                Panel0_UseKey.gameObject.SetActive(false);
                Panel1_NeedKey.gameObject.SetActive(false);
                Panel2_NeedEnergy.gameObject.SetActive(true);
                Panel3_DoorOpened.gameObject.SetActive(false);
                break;
            case 3:
                Panel0_UseKey.gameObject.SetActive(false);
                Panel1_NeedKey.gameObject.SetActive(false);
                Panel2_NeedEnergy.gameObject.SetActive(false);
                Panel3_DoorOpened.gameObject.SetActive(true);
                break;
            default:
                Panel0_UseKey.gameObject.SetActive(false);
                Panel1_NeedKey.gameObject.SetActive(false);
                Panel2_NeedEnergy.gameObject.SetActive(false);
                Panel3_DoorOpened.gameObject.SetActive(false);
                Debug.Log("ChangePopup with index " + index + " defaulted.");
                break;
        }
    }

    public void DisableCanvas()
    {
        this.GetComponent<Canvas>().enabled = false;
        isShown = false;
    }

    public void EnableCanvas()
    {
        this.GetComponent<Canvas>().enabled = true;
        isShown = true;
        ChangePopup(WhichPopup());
    }


}
