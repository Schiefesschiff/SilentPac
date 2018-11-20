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


    private readonly int m_HashHorizontPara = Animator.StringToHash("Horizontal");
    private readonly int m_HashVerticalPara = Animator.StringToHash("Vertical");

    private readonly int m_HashRotationX = Animator.StringToHash("MouseX");
    private readonly int m_HashRotationY = Animator.StringToHash("MouseY");

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            rotationY = Input.GetAxis("Mouse Y");
            rotationX = Input.GetAxis("Mouse X");



        Vector2 input = new Vector2(horizontal, vertical).normalized;
        Vector2 InputRotation = new Vector2(rotationX, rotationY).normalized;

        if (!Input.anyKey)
        {

        }

        animator.SetFloat(m_HashHorizontPara, input.x, m_Damping, Time.deltaTime);
        animator.SetFloat(m_HashVerticalPara, input.y, m_Damping, Time.deltaTime);

        animator.SetFloat(m_HashRotationX, InputRotation.x, m_Damping, Time.deltaTime);
        animator.SetFloat(m_HashRotationY, InputRotation.y, m_Damping, Time.deltaTime);
    }


}
