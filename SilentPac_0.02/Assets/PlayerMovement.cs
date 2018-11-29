using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float turnSmoothing = 0.15f;
    public float speedDampTime = 0.1f;
    public bool run;

    private float Speed;

    public GameObject camObj;
    private Rigidbody rig;
    private Animator anim;

    private void Awake()
    {

        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        run = Input.GetButton(StringCollection.INPUT_B);      // b 

        Speed = Mathf.Sqrt(h * h + v * v);      // speed from stick ( controller)

        MovementManagement(h, v , run, Speed);
    }
 
    void MovementManagement(float horizontal, float vertical, bool isRunning, float speed)
    {
        anim.SetBool("Run", isRunning);

        if (horizontal != 0f || vertical != 0f)
        {
            Rotating(horizontal, vertical);
            anim.SetFloat("Vertical", speed, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Vertical", 0, speedDampTime, Time.deltaTime);
        }
    }
    
    void Rotating(float horizontal, float vertical)     //  to do wenn eingabe null player dreht sichnicht mit!
    {

        //float heading = (Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg);    //  output = degrees

        //transform.rotation = Quaternion.Euler(0f, 0f, heading * Mathf.Rad2Deg);

        Vector3 targetDirection = new Vector3(horizontal, 0, vertical);
                     
        Vector3 worldDirection = transform.TransformDirection(targetDirection);
        print(worldDirection);
        //targetDirection = transform.TransformPoint(targetDirection); from word to local
        Quaternion targetRotation = Quaternion.LookRotation(worldDirection, transform.up);
        
        Quaternion newRotation = Quaternion.Slerp(rig.transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        rig.MoveRotation(newRotation);
    }

    


}
