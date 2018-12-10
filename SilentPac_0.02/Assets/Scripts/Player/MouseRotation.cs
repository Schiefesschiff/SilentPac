using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Camera-Control/Smooth Mouse Look")]
public class MouseRotation : MonoBehaviour
{
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;
    float rotationY = 0F;

    //private List<float> rotArrayX = new List<float>();
    //float rotAverageX = 0F;

    //private List<float> rotArrayY = new List<float>();
    //float rotAverageY = 0F;

    //public float frameCounter = 20;

    private Quaternion originalRotation;

    void Update()
    {

        //rotAverageY = 0f;
        //rotAverageX = 0f;


        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;

        rotationY += Input.GetAxis(StringCollection.INPUT_RVERTICAL) * sensitivityY;
        rotationX += Input.GetAxis(StringCollection.INPUT_RHORIZONTAL) * sensitivityX;

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!  für die Smoothness  !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


        //rotArrayY.Add(rotationY);
        //rotArrayX.Add(rotationX);

        //if (rotArrayY.Count >= frameCounter)
        //{
        //    rotArrayY.RemoveAt(0);
        //}
        //if (rotArrayX.Count >= frameCounter)
        //{
        //    rotArrayX.RemoveAt(0);
        //}

        //for (int j = 0; j < rotArrayY.Count; j++)
        //{
        //    rotAverageY += rotArrayY[j];
        //}
        //for (int i = 0; i < rotArrayX.Count; i++)
        //{
        //    rotAverageX += rotArrayX[i];
        //}

        //rotAverageY /= rotArrayY.Count;
        //rotAverageX /= rotArrayX.Count;

        //rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
        //rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            rotationY = ClampAngle(rotationY, minimumY, maximumY);
            rotationX = ClampAngle(rotationX, minimumX, maximumX);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);  //rotAverageY beim ersten AngleAxis argument eintagen
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);    //rotAverageX beim ersten AngleAxis argument eintragen

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        
    }

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.freezeRotation = true;
        }
        originalRotation = transform.localRotation;
    }

    public float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}
