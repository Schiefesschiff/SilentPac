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
    private bool lastInput = true;
    private Quaternion targetRotation;
    private Vector3 targetPos;
    public float angle;
    private Vector2 input;
    private float MoveAxeX;
    private float MoveAxeY;
    private float rotationX;
    private float rotationY;

    private void FixedUpdate()
    {
        InputCameraControll();

        if (!Alarm)
        {
            RotateARoundTarget(rotationX);
            MoveWithTarget();
            LookAtTarget();
            RotateBehindPlayer();
        }
        else
        {
            MoveToSecondPos();
        }

    }

    void MoveToSecondPos()
    {
        if (transform.position != SecondCameraPos.transform.position)
        {
           transform.position = Vector3.Lerp(transform.position, SecondCameraPos.transform.position, 1.5f * Time.deltaTime);
           transform.rotation = Quaternion.Slerp(transform.rotation, SecondCameraPos.transform.rotation, 1 * Time.deltaTime);
        }
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

    void RotateARoundTarget(float angle)
    {
        Vector3 vel = Vector3.zero;
        Vector3 targetOffsetPos = Quaternion.Euler(0, angle * rotationSpeed, 0) * offsetPos;
        float dist = Vector3.Distance(offsetPos, targetOffsetPos);

        while (dist > 0.02f)
        {
            offsetPos = Vector3.SmoothDamp(offsetPos, targetOffsetPos, ref vel, smoothSpeed);
            dist = Vector3.Distance(offsetPos, targetOffsetPos);
        }

        offsetPos = targetOffsetPos;

    }


    private bool isInputRighJoy()
    {
        if (rotationX != 0 || rotationY != 0)
        {
            return false;
        }

        if (input.y != 0 || input.y != 0)
        {
            lastInput = true;
            return true;
        }
        return false;

        //if (rotationX != 0 || rotationY !=0)
        //{
        //    lastInput = false;
        //    return false;
        //}

        //if (input.y != 0 || input.y != 0)
        //{
        //    lastInput = true;
        //    return true;
        //}

        //if (lastInput == true)
        //{
        //    return true;
        //}

        //return false;

    }

    void RotateBehindPlayer()
    {
        Vector3 direction = transform.position - target.position;

        angle = Vector3.SignedAngle(direction, -target.forward, target.up);

        if (isInputRighJoy())
        {
            if (angle <= -67 )
            {
                Vector3 targetOffsetPos = Quaternion.Euler(0, -1, 0) * offsetPos;
                offsetPos = targetOffsetPos;
            }
            else if (angle >= 67 )
            {
                Vector3 targetOffsetPos = Quaternion.Euler(0, 1, 0) * offsetPos;
                offsetPos = targetOffsetPos;
            }
        }
        //print("angle " + angle);
        print(input);
    }

}
