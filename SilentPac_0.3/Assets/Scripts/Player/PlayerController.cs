using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerController : MonoBehaviour
{
    public Transform camTarget;
    public GameObject ParticelPullEnergy;
    public float  turnSpeed = 10f;
    public float speedDampTime = 0.1f;
    public float RecoverEnergySpeed = 0.5f;
    public Transform ParticelPos;
    private Vector2 input;
    private float angle;
    private Animator anim;
    private PlayerEnergy playerEnergy;
    private float ForwardSpeed;
    private Quaternion targetRotation;
    private Transform cam;
    private bool run;
    public bool pullEnergy;
    private bool shoot;
    private bool ArcadeSight;
    private Vector3 curCamTargetPos;
    public SphereCollider sphereCol;
    public List<Transform> forks = new List<Transform>();
    public List<Transform> enemiesClose = new List<Transform>();


    private void Start()
    {
        sphereCol = transform.GetChild(0).GetComponent<SphereCollider>();
        playerEnergy = GetComponent<PlayerEnergy>();
        cam = Camera.main.transform.transform;
        anim = GetComponent<Animator>();
        curCamTargetPos = camTarget.position;
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

        if (!cam.GetComponent<CameraController>().stopMove)
        {
            Move();
        }
        else
        {
            input = new Vector2(0, 0);
            Move();
        }
    }

    #region Test Forks for Enemy2 (Collider)
    public Vector3 NextWayPoint()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < forks.Count; i++)
        {
            Vector3 direction = forks[i].transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < 100 * 0.5f)
            {
                pos = forks[i].transform.position;
            }
        }
        return pos; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Forks")
        {
            forks.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Forks")
        {
            forks.Remove(other.transform);
        }
    }
    #endregion

    void GetInput()
    {
        input.x = Input.GetAxisRaw(StringCollection.INPUT_HORIZONTAL);
        input.y = Input.GetAxisRaw(StringCollection.INPUT_VERTICAL);
        //shoot = Input.GetButton(StringCollection.INPUT_X);
        pullEnergy = Input.GetButton(StringCollection.INPUT_RB);

        run = Input.GetButton(StringCollection.INPUT_LB);

        if (Input.GetButtonDown(StringCollection.INPUT_X) || Input.GetKeyDown(KeyCode.LeftShift))
        {
            shoot = true;
        }

        if (cam.transform.GetComponent<CameraController>().ArcadeSight || Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
    }

    #region Actions Functions

    void PullEnergy(bool pull)
    {
        if (pull)
        {
            if (anim.GetBool("Shoot") == false )
            {
                //print("pull");
                anim.SetBool("PullEnergy", true);
                ParticelPullEnergy.SetActive(true);

                playerEnergy.AddStamina(RecoverEnergySpeed);

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
        if (shoot && playerEnergy.currentHealth > 0)
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

    #endregion

    // direction relativ to the camera`s Rotation
    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, input.y);              // give Radians back
        angle = Mathf.Rad2Deg * angle;                      // make radians to degrees
        angle += cam.eulerAngles.y;      // rotation relative to camera
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
