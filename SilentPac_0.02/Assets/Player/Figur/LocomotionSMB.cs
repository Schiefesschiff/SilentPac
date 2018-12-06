using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotionSMB : StateMachineBehaviour
{
    public float m_Damping = 0.15f;


    float horizontal;
    float vertical;

    float rotationY;
    float rotationX;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {

    }
    
    private readonly int m_HashHorizontPara = Animator.StringToHash(StringCollection.INPUT_HORIZONTAL);
    private readonly int m_HashVerticalPara = Animator.StringToHash(StringCollection.INPUT_VERTICAL);

    private readonly int m_HashRotationX = Animator.StringToHash(StringCollection.INPUT_RHORIZONTAL);
    private readonly int m_HashRotationY = Animator.StringToHash(StringCollection.INPUT_RVERTICAL);

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        horizontal = Input.GetAxis(StringCollection.INPUT_HORIZONTAL);
        vertical = Input.GetAxis(StringCollection.INPUT_VERTICAL);

        rotationY = Input.GetAxis(StringCollection.INPUT_RHORIZONTAL);
        rotationX = Input.GetAxis(StringCollection.INPUT_RVERTICAL);


        Vector2 input = new Vector2(horizontal, vertical).normalized;
        Vector2 InputRotation = new Vector2(rotationX, rotationY).normalized;

        // player.LookAt(camera.forward + (vertical, horizontal));
        


        if (!Input.anyKey)
        {

        }

        animator.SetFloat(m_HashHorizontPara, input.x, m_Damping, Time.deltaTime);
        animator.SetFloat(m_HashVerticalPara, input.y, m_Damping, Time.deltaTime);

        //animator.SetFloat(m_HashRotationX, InputRotation.x, m_Damping, Time.deltaTime);
        //animator.SetFloat(m_HashRotationY, InputRotation.y, m_Damping, Time.deltaTime);
        /*
        if (input != Vector2.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(input), 0.2f);

        }*/

        // if (movement != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.normalized), 0.2f);
        
        // Vector3.RotateTowards
    }


}