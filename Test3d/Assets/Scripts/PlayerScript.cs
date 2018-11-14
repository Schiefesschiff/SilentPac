using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public GameObject startPositionObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            transform.position = startPositionObj.transform.position;
        }
    }

}
