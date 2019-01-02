using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool useController = true;
    
    void FixedUpdate()
    {

        if (useController)
        {
            //Left Joystick: Move control
            float moveHorizontal = Input.GetAxis(StringCollection.INPUT_HORIZONTAL);
            float moveVertical = Input.GetAxis(StringCollection.INPUT_VERTICAL);

            //Right Joystick: Rotation control
            float rotateHorizontal = Input.GetAxis(StringCollection.INPUT_RHORIZONTAL);
            float rotateVertical = Input.GetAxis(StringCollection.INPUT_RVERTICAL);

            /* //rotate player to RStickposition
            Vector3 playerDirection = Vector3.right * Input.GetAxisRaw(StringCollection.INPUT_RHORIZONTAL) + Vector3.forward * -Input.GetAxisRaw(StringCollection.INPUT_RVERTICAL);
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);

            }
            */

            //Buttons A/B/X/Y

            if (Input.GetButtonDown(StringCollection.INPUT_A))
            {
                //use glove*
                Debug.Log("A");
            }

            if (Input.GetButtonDown(StringCollection.INPUT_B))
            {
                //shoot charge
                Debug.Log("B");

            }

            if (Input.GetButtonDown(StringCollection.INPUT_X))
            {
                //drop charge
                Debug.Log("X");

            }

            if (Input.GetButtonDown(StringCollection.INPUT_Y))
            {
                //not used
                Debug.Log("Y");

            }

            //Buttons LB/RB

            if (Input.GetButtonDown(StringCollection.INPUT_LB))
            {
                //not used
                Debug.Log("LB");

            }

            if (Input.GetButtonDown(StringCollection.INPUT_RB))
            {
                //not used
                Debug.Log("RB");

            }

            //Buttons LT/RT

            if (Input.GetAxis(StringCollection.INPUT_LT) != 0)
            {
                //run
                Debug.Log("LT");

            }

            if (Input.GetAxis(StringCollection.INPUT_RT) != 0)
            {
                //activate glove
                Debug.Log("RT");

            }

            //Buttons Select/Start

            if (Input.GetButtonDown(StringCollection.INPUT_SELECT))
            {
                //not used
                Debug.Log("Select");

            }

            if (Input.GetButtonDown(StringCollection.INPUT_START))
            {
                //not used
                Debug.Log("Start");

            }

            //Buttons Stickpress

            if (Input.GetButtonDown(StringCollection.INPUT_LSTICKPRESS))
            {
                //not used
                Debug.Log("LStickpress");

            }

            if (Input.GetButtonDown(StringCollection.INPUT_RSTICKPRESS))
            {
                //not used
                Debug.Log("RStickpress");

            }


        }
    }
}