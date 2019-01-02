using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetup
{
    public float speedDampTime = 0.1f;
    public float angularSpeedDampTime = 0.7f;
    public float angleResponseTime = 0.6f;

    private Animator anim;
    //private string stringName;

    public AnimatorSetup(Animator animator)
    {
        anim = animator;
        //stringName = _stringName;
    }

    public void Setup(float speed, float angle)
    {
        float angularSpeed = angle / angleResponseTime;

        anim.SetFloat("Forward", speed, speedDampTime, Time.deltaTime);
        anim.SetFloat("Turn", angularSpeed, angularSpeedDampTime, Time.deltaTime);
    }
}

