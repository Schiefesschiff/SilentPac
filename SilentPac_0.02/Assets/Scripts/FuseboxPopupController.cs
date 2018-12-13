using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseboxPopupController : MonoBehaviour
{
    private PlayerInventory playerInventory;
    private GameObject player;
    private FuseboxController fuseboxController;
    public GameObject fusebox;

    public bool isShown = true;

    public GameObject Panel0_UseFuse;
    public GameObject Panel1_NeedFuse;
    public GameObject Panel2_FuseboxRepaired;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();

        fuseboxController = fusebox.GetComponent<FuseboxController>();

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
        if (fuseboxController.isRepaired)
            return 2;
        else if (!playerInventory.hasFuse)
            return 1;
        else if (playerInventory.hasFuse)
            return 0;

        return 0;
    }

    public void ChangePopup(int index)
    {
        switch (index)
        {
            case 0:
                Panel0_UseFuse.gameObject.SetActive(true);
                Panel1_NeedFuse.gameObject.SetActive(false);
                Panel2_FuseboxRepaired.gameObject.SetActive(false);
                break;
            case 1:
                Panel0_UseFuse.gameObject.SetActive(false);
                Panel1_NeedFuse.gameObject.SetActive(true);
                Panel2_FuseboxRepaired.gameObject.SetActive(false);
                break;
            case 2:
                Panel0_UseFuse.gameObject.SetActive(false);
                Panel1_NeedFuse.gameObject.SetActive(false);
                Panel2_FuseboxRepaired.gameObject.SetActive(true);
                break;
            default:
                Panel0_UseFuse.gameObject.SetActive(false);
                Panel1_NeedFuse.gameObject.SetActive(false);
                Panel2_FuseboxRepaired.gameObject.SetActive(false);
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
