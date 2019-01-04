using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen;

    //private Vector3 moveDoor;
    
    void Start()
    {
        isOpen = false;

        //moveDoor = new Vector3 (0f, 10f, 0f);
    }

    public void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("I was opened X.X (I'm a door btw)");
            //Destroy(this);
            Destroy(gameObject);
        }
    }    
}
