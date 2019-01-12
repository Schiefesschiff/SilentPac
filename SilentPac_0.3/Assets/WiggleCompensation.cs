using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleCompensation : MonoBehaviour
{
    public float strenght = 0.5f;
    public Transform target;
    public float heigth;

    private void Update()
    {

        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x , target.position.y + heigth, target.position.z), strenght * Time.deltaTime);
    }


}
