using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooropenerInteraction : MonoBehaviour
{
    //public AudioClip keyDrop;

    private GameObject player;
    private PlayerInventory playerInventory;
    public GameObject door;
    private DoorController doorController;
    private GameObject radar;
    private GoalRadarController goalRadarController;
    public HudController hudController;
    public Canvas canvas;
    public DooropenerPopupController dooropenerPopupController;

    private bool showPopup;
    public bool isDoorOpen;
    public bool hasEnergy;

    void Awake()
    {
        hudController = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>();

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();

        door = GameObject.FindGameObjectWithTag("Door");
        doorController = door.GetComponent<DoorController>();
        
        dooropenerPopupController = canvas.GetComponent<DooropenerPopupController>();

        radar = GameObject.FindGameObjectWithTag("Radar");
        goalRadarController = radar.GetComponent<GoalRadarController>();
        
        showPopup = false;
        isDoorOpen = false;
        hasEnergy = false;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            showPopup = true;
            dooropenerPopupController.EnableCanvas();

            if (playerInventory.hasKey)
                hudController.MakeButtonBright(hudController.buttonImage_A);
            
            //AudioSource.PlayClipAtPoint(keyDrop, transform.position);         
        }
        
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
            hudController.MakeButtonDark(hudController.buttonImage_A);
    }

    void OnTriggerStay (Collider other)
    {     
        if (other.gameObject == player)
        {
            if (!hasEnergy)
            {
                Debug.Log("Repair the fusebox to give energy to this terminal.");
            }
            else
            {
                if (showPopup && !isDoorOpen)
                {
                    if (playerInventory.hasKey)
                        Debug.Log("Press A to open door.");
                    else
                        Debug.Log("You need the key to open the door.");
                }

                if (playerInventory.hasKey && Input.GetButtonDown(StringCollection.INPUT_A) && !isDoorOpen)
                {
                    OpenSesame();
                }

                //AudioSource.PlayClipAtPoint(keyDrop, transform.position);
            }
        }
        
    }

    public void OpenSesame()
    {
        playerInventory.RemoveKeyFromInventory();
        hudController.RemoveKeyFromInventoryUI();
        doorController.OpenDoor();
        isDoorOpen = true;
        goalRadarController.GoToNextAvailableWaypoint();
        Debug.Log("I opened the door (sneak, totally didn't)");
        dooropenerPopupController.ChangePopup(3);
    }

    public void TurnOn()
    {
        hasEnergy = true;
        Debug.Log("Dooropener hasEnergy. :)");
    }

    public void TurnOff()
    {
        hasEnergy = false;
        dooropenerPopupController.ChangePopup(2);
        Debug.Log("Dooropener hasn'tEnergy. :(");
    }
    
}
