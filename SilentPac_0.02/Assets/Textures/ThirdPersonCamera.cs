using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform target;
    public float dstFromTarget = 2;
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public float rotationSmoothTime = 0.12f;

    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;


    private float yaw;
    public float AngleCamera;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        print(yaw);

        yaw += Input.GetAxis(StringCollection.INPUT_RHORIZONTAL) * mouseSensitivity;
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;

        print(yaw);
        //pitch -= Input.GetAxis(StringCollection.INPUT_RVERTICAL);
        //pitch -= Input.GetAxis("Mouse Y")* mouseSensitivity;
        //pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(AngleCamera, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * dstFromTarget;
    }
}

