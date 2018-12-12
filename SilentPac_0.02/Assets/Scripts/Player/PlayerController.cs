using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


public class PlayerController : MonoBehaviour
{
    public GameObject ParticelPullEnergy;
    public float  turnSpeed = 10f;
    public float speedDampTime = 0.1f;
    public Transform ParticelPos;
    private Vector2 input;
    private float angle;
    private Animator anim;
    private PlayerEnergy playerEnergy;
    private float ForwardSpeed;
    private Quaternion targetRotation;
    private Transform cam;
    private bool run;
    private bool pullEnergy;
    private bool shoot;
    private bool ArcadeSight;

    private void Start()
    {
        playerEnergy = GetComponent<PlayerEnergy>();
        cam = Camera.main.transform.transform;
        anim = GetComponent<Animator>();

    }
       
    private void Update()
    {
            GetInput();
            Run(run);
            Shoot(shoot);
            PullEnergy(pullEnergy);

            if (input.x != 0f || input.y != 0f)
            {
                CalculateDirection();
                Rotation();
            }
            Move();
    }

    // input base on horizontal (a,b,<,>) and vertical(w,s,^,v) keys
    void GetInput()
    {
        input.x = Input.GetAxisRaw(StringCollection.INPUT_HORIZONTAL);
        input.y = Input.GetAxisRaw(StringCollection.INPUT_VERTICAL);
        //print(input);
        run = Input.GetButton(StringCollection.INPUT_LB);      
        shoot = Input.GetButton(StringCollection.INPUT_X);
        cam.GetComponent<CameraController>().ArcadeSight = Input.GetButton(StringCollection.INPUT_RB);
        pullEnergy = Input.GetButton(StringCollection.INPUT_RB);
    }

    void PullEnergy(bool pull)
    {
        if (pull)
        {
            if (anim.GetBool("Shoot") == false )
            {
                anim.SetBool("PullEnergy", true);
                ParticelPullEnergy.SetActive(true);
                playerEnergy.AddStamina(10);
            }
            
        }
        else
        {
            anim.SetBool("PullEnergy", false);
            ParticelPullEnergy.SetActive(false);
        }
    }

    void Run(bool isRun)
    {
        if (isRun)
        {
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    void Shoot(bool shoot)
    {
        if (shoot && playerEnergy.currentHealth >= 0)
        {
            anim.SetBool("Shoot", true);
            anim.SetBool("PullEnergy", false);
            ParticelPullEnergy.SetActive(true);
        }
        else
        {
            anim.SetBool("Shoot", false);

        }
    }

    // direction relativ to the camera`s Rotation
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);              // give Radians back
        angle = Mathf.Rad2Deg * angle;                      // make radians to degrees
        angle += cam.eulerAngles.y;                         // rotation relative with camera
    }

    //rotate toward the calculate angle
    void Rotation()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);     // convert the euler angels to Quaternion
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        
    }

    // this Player only move along its forward axis
    void Move()
    {
        CalculateSpeed(input);
        //transform.position += transform.forward * velocity * Time.deltaTime;
        anim.SetFloat("Vertical", ForwardSpeed, speedDampTime, Time.deltaTime);

    }

    void CalculateSpeed(Vector2 input)
    {
        ForwardSpeed = Mathf.Sqrt(input.x * input.x + input.y * input.y);      // speed from stick ( controller)
        //print(ForwardSpeed);
    }
}
