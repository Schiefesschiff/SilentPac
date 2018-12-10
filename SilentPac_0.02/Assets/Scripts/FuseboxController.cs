using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseboxController : MonoBehaviour
{
    public bool isRepaired;
    private bool showTooltip;

    private GameObject player;
    private PlayerInventory playerInventory;
    public GameObject dooropener;
    private DooropenerInteraction dooropenerInteraction;
    public HudController hudController;



    void Awake()
    {
        isRepaired = false;
        showTooltip = true;

        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();

        dooropener = GameObject.FindGameObjectWithTag("Dooropener");
        dooropenerInteraction = dooropener.GetComponent<DooropenerInteraction>();        
    }

    public void RepairFusebox()
    {
        if (!isRepaired)
        {
            isRepaired = true;
            Debug.Log("I was repaired! :) (I'm a fusebox.)");
            hudController.removeFuseFromInventoryUI();

            dooropenerInteraction.TurnOn();
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {            
                if (showTooltip && !isRepaired)
                {
                    if (playerInventory.hasFuse)
                        Debug.Log("Press A to insert fuse.");
                    else
                        Debug.Log("You need the fuse to repair this fusebox.");
                }

                if (playerInventory.hasFuse && Input.GetButtonDown(StringCollection.INPUT_A) && !isRepaired)
                {
                    playerInventory.hasFuse = false;
                    showTooltip = false;

                    RepairFusebox();

                    Debug.Log("Fusebox repaired.");
                }

                //AudioSource.PlayClipAtPoint(keyDrop, transform.position);            
        }

    }
    
}
