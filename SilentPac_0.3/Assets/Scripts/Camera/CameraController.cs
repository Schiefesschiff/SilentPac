using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;
    public Transform target;
    public Vector3 offsetPos;
    public float AutoSmoothnessCameraMove = 5f;
    public float AutoSmoothnessLookAtSpeed = 10f;
    public float rightStickRotationSpeed = 10f;
    public float RotateSlopeSpeed = 4f;
    public float maxSlopAngle = 6f;
    public GameObject SecondCameraPos;

    public bool Alarm = false;      // control from LastPlayerSightning 
    public bool ArcadeSight;
    private Quaternion targetRotation;
    private Vector3 targetPos;
    private float angle;
    private Vector2 input;
    private float MoveAxeX;
    private float MoveAxeY;
    private float rotationX;
    private float rotationY;
    private float minimumY;
    public GameObject secondCamera;
    public float minDistanceCamPlayer;

    public float timeTakenDuringLerp = 1f;      // how long zoom down (time)
    private float _timeStartedLerping;
    private Vector3 _startPosition;                     // start position for second camera
    public bool stopArcadeMode = false;

    private void Awake()
    {
        //secondCamera = this.gameObject.transform.GetChild(0).gameObject;
        minimumY = offsetPos.y;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        CameraCollision();
    }

    private void FixedUpdate()
    {
        InputCameraControll();

        if (Input.GetButtonDown(StringCollection.INPUT_Y))
        {
            SetCameraBehind();
        }

        RotateARoundTarget(rotationX);
        MoveWithTarget();
        LookAtTarget();
        //RotateBehindPlayer();
        RotateSlope(rotationY);

        if(ArcadeSight)
        {
           MoveToSecondPos();
        }
        else
        {
            ReturnToNormalPos();
        }

    }


    
    public bool ResetCameraPos()
    {
        if (!stopArcadeMode)
        {
            return false;
        }
        else
        {

            return true;
        }
    }

    public float CamEulerAngel()        // for playercontrollern (move)
    {
        if (ArcadeSight)
        {
            return secondCamera.transform.eulerAngles.y;
        }

        return transform.eulerAngles.y;

    }

    void MoveToSecondPos()
    {
        secondCamera.SetActive(true);
        secondCamera.transform.position = Vector3.Lerp(secondCamera.transform.position, target.position + new Vector3(0,35,0), 5 * Time.deltaTime);
        secondCamera.transform.eulerAngles = new Vector3(90, 0, 0);

        SetCameraBehind();

        _timeStartedLerping = Time.time;
        _startPosition = secondCamera.transform.position;
    }

    void ReturnToNormalPos()
    {
        if (secondCamera.activeSelf)
        {
            stopArcadeMode = true;
            float timeSinceStarted = Time.time - _timeStartedLerping;
            float percentageComplete = timeSinceStarted / timeTakenDuringLerp;

            targetPos = target.position + offsetPos;
            secondCamera.transform.position = Vector3.Lerp(_startPosition, targetPos, percentageComplete);

            targetRotation = Quaternion.LookRotation(target.position - secondCamera.transform.position);
            secondCamera.transform.rotation = Quaternion.Slerp(secondCamera.transform.rotation, targetRotation, percentageComplete);

            if (percentageComplete >= 1.0f)
            {
                secondCamera.SetActive(false);
                StartCoroutine(StopArcadeMode());
                //secondCamera.SetActive(false);
                //stopArcadeMode = false;
            }
        }
    }

    IEnumerator StopArcadeMode()
    {
        yield return new WaitForSeconds(0.2f);
        stopArcadeMode = false;
    }

    void InputCameraControll()
    {
        rotationX = Input.GetAxis(StringCollection.INPUT_RHORIZONTAL);        // right sticks
        rotationY = Input.GetAxis(StringCollection.INPUT_RVERTICAL);

        input.x = Input.GetAxis(StringCollection.INPUT_HORIZONTAL);         // left Sticks
        input.y = Input.GetAxis(StringCollection.INPUT_VERTICAL);
        ArcadeSight = Input.GetButton(StringCollection.INPUT_RB);
    }

    // Move Camera to the player position + current offset
    //offset is modified by the rotationAroundTarget coroutine
    void SetCameraBehind()
    {
        Vector3 direction = transform.position - new Vector3(target.position.x, transform.position.y, target.position.z);

        angle = Vector3.SignedAngle(direction, -target.forward, target.up);

        targetPos = target.position + offsetPos;            // distance with offset
        transform.position = Vector3.Lerp(transform.position, targetPos, AutoSmoothnessCameraMove * Time.deltaTime);


        Vector3 targetOffsetPos = Quaternion.Euler(0, angle, 0) * offsetPos;
        offsetPos = targetOffsetPos;
        print(angle);
    }

    void MoveWithTarget()
    {
        targetPos = target.position + offsetPos;            // distance with offset
        transform.position = Vector3.Lerp(transform.position, targetPos, AutoSmoothnessCameraMove * Time.deltaTime);
    }

    // use look vector (target - current) to aim the cameratoward the player
    void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, AutoSmoothnessLookAtSpeed * Time.deltaTime);
    }

    void RotateARoundTarget(float angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle * rightStickRotationSpeed,0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);

        while (dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, 5);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
        }

        offsetPos = targetOffsetPos;
    }

    void RotateSlope(float angle)
    {
        //Vector3 vel = Vector3.zero;
        //Vector3 targetOffsetPos = Quaternion.Euler(0, 0, -angle * RotateSlopeSpeed) * offsetPos;

        //offsetPos = targetOffsetPos;

        offsetPos = offsetPos + new Vector3(0, angle * RotateSlopeSpeed, 0);
        offsetPos.y = Mathf.Clamp(offsetPos.y, minimumY, maxSlopAngle);
    }

    private bool isInputRighJoy()
    {
        if (rotationX != 0 || rotationY != 0)
        {
            return false;
        }

        if (input.y != 0 || input.x != 0)
        {
            return true;
        }
        return false;
    }

    void RotateBehindPlayer()
    {
        Vector3 direction = transform.position - new Vector3(target.position.x , transform.position.y, target.position.z);

        angle = Vector3.SignedAngle(direction, -target.forward, target.up);

        if (isInputRighJoy())
        {
            if (angle <= -5 )
            {
                Vector3 targetOffsetPos = Quaternion.Euler(0, -1, 0) * offsetPos;
                offsetPos = targetOffsetPos;
            }
            else if (angle >= 5 )
            {
                Vector3 targetOffsetPos = Quaternion.Euler(0, 1, 0) * offsetPos;
                offsetPos = targetOffsetPos;
            }
        }
        //print("angle " + angle);
        //print(input);
    }
    
    void CameraCollision()
    {
        Vector3 dir = target.position - new Vector3(transform.position.x , target.position.y , transform.position.z);

        RaycastHit  hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
        {
            offsetPos = offsetPos + dir.normalized * 0.1f;
        }

        float dis = Vector3.Distance(target.position, new Vector3(transform.position.x , target.position.y , transform.position.z));

        if (minDistanceCamPlayer >= dis)
        {
            Vector3 newPos = target.position + offsetPos - dir.normalized * 0.1f;
            Debug.DrawLine(target.position, newPos); 

            if (!Physics.Linecast(target.position, newPos, out hit))
            {
                //print(hit.collider.name);
                offsetPos = offsetPos - dir.normalized * 0.1f;
            }
        }
        //print("distance " + dis);
    }
}
