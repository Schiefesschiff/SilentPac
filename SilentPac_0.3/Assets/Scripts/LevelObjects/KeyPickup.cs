using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    //public AudioClip keyGrab;

    private GameObject player;
    private PlayerInventory playerInventory;
    private GameObject radar;
    private GoalRadarController goalRadarController;
    public HudController hudController;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();
        radar = GameObject.FindGameObjectWithTag("Radar");
        goalRadarController = radar.GetComponent<GoalRadarController>();
        hudController = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            //AudioSource.PlayClipAtPoint(keyGrab, transform.position);
            playerInventory.AddKeyToInventory();
            hudController.AddKeyToInventoryUI();
            goalRadarController.GoToNextAvailableWaypoint();
            Debug.Log("key says bye");
            Destroy(gameObject);                
        }
    }
    
}
