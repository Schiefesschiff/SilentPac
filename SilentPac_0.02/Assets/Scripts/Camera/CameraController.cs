using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float SmoothnessCameraMove = 5f;
    public float turnSpeed = 10f;
    public float smoothSpeed = 5f;
    public float rotationSpeed = 10;
    public GameObject SecondCameraPos;

    public bool Alarm = false;
    public bool ArcadeSight = false;
    private bool lastInput = true;
    private Quaternion targetRotation;
    private Vector3 targetPos;
    public float angle;
    private Vector2 input;
    private float MoveAxeX;
    private float MoveAxeY;
    private float rotationX;
    private float rotationY;


    public float minimumY = -60F;
    public float maximumY = 60F;


    private void FixedUpdate()
    {
        InputCameraControll();

        if (!ArcadeSight)
        {
            RotateARoundTarget(rotationX, rotationY);
            MoveWithTarget();
            LookAtTarget();
            RotateBehindPlayer();
            RotateSlope(rotationY);
        }
        else
        {
            MoveToSecondPos();
        }

    }

    void MoveToSecondPos()
    {
        float dist = Vector3.Distance(transform.position, SecondCameraPos.transform.position);

        transform.position = Vector3.Lerp(transform.position, SecondCameraPos.transform.position, 3f * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, SecondCameraPos.transform.rotation, 3 * Time.deltaTime);
        Vector3 targetPos = new Vector3(target.position.x, SecondCameraPos.transform.position.y, target.position.z);
        SecondCameraPos.transform.position = Vector3.Lerp(SecondCameraPos.transform.position, targetPos , 1f);

    }

    void InputCameraControll()
    {
        rotationX = Input.GetAxis(StringCollection.INPUT_RHORIZONTAL);        // right sticks
        rotationY = Input.GetAxis(StringCollection.INPUT_RVERTICAL);

        input.x = Input.GetAxis(StringCollection.INPUT_HORIZONTAL);         // left Sticks
        input.y = Input.GetAxis(StringCollection.INPUT_VERTICAL);

    }

    // Move Camera to the player position + current offset
    //offset is modified by the rotationAroundTarget coroutine
    void MoveWithTarget()
    {
        targetPos = target.position + offsetPos;            // distance with offset
        transform.position = Vector3.Lerp(transform.position, targetPos, SmoothnessCameraMove * Time.deltaTime);
    }

    // use look vector (target - current) to aim the cameratoward the player
    void LookAtTarget()
    {
        targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void RotateARoundTarget(float angle, float angleY)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle * rotationSpeed,0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);

        while (dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
        }

        offsetPos = targetOffsetPos;
    }


    void RotateSlope(float angle)
    {
        offsetPos = offsetPos + new Vector3(0, angle / 4 ,0);
        offsetPos.y = Mathf.Clamp(offsetPos.y, minimumY, maximumY);
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
        print("angle " + angle);
        //print(input);
    }


}
