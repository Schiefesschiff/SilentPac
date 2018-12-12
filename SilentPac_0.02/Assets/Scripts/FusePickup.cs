﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusePickup : MonoBehaviour
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
            playerInventory.AddFuseToInventory();
            hudController.AddFuseToInventoryUI();
            Debug.Log("fuse says bye");
            Destroy(gameObject);
        }        
    }

}
