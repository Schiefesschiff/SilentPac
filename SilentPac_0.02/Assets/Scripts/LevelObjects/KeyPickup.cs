using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    //public AudioClip keyGrab;

    private GameObject player;
    private PlayerInventory playerInventory;
    public HudController hudController;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInventory = player.GetComponent<PlayerInventory>();        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            //AudioSource.PlayClipAtPoint(keyGrab, transform.position);
            playerInventory.AddKeyToInventory();
            hudController.AddKeyToInventoryUI();
            Debug.Log("key says bye");
            Destroy(gameObject);                
        }
    }
    
}
