using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class EnemyAnimation : MonoBehaviour
{
    //public float deadZone = 5f;

    private Transform player;
    private EnemySight enemySight;
    private NavMeshAgent nav;
    private Animator anim;
    private AnimatorSetup animSetup;

    private ThirdPersonCharacter character;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        character = GetComponent<ThirdPersonCharacter>();

        nav.updateRotation = false;
        //animSetup = new AnimatorSetup(anim);
        //deadZone *= Mathf.Deg2Rad;
    }

    //private void Update()
    //{
    //    //NavAnimSetup();


    //    if (nav.remainingDistance > nav.stoppingDistance)       // distance between enemy / player
    //    {
    //        character.Move(nav.desiredVelocity, false, false,speed);      // for third person controller(animation)
    //    }
    //    else
    //    {
    //        character.Move(Vector3.zero, false, false, speed);
    //    }
    //}

    private void OnAnimatorMove()
    {
        //nav.velocity = anim.deltaPosition / Time.deltaTime;
        //transform.rotation = anim.rootRotation;
    }

    //void NavAnimSetup()
    //{
    //    float speed;
    //    float angle;

    //    if (enemySight.playerInSight)
    //    {
    //        speed = 0;

    //        angle = FindAngle(transform.forward, player.position - transform.position, transform.up);
    //    }
    ////    else
    //    {
    //speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;

    //        angle = FindAngle(transform.forward, nav.desiredVelocity, transform.up);

    //        if (Mathf.Abs(angle) < deadZone)
    //        {
    //            transform.LookAt(transform.position + nav.desiredVelocity);     // control navMesgh lookat function
    //            angle = 0f;
    //        }
    //    }

    //    animSetup.Setup(speed, angle);
    //}

    //float FindAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    //{
    //    if (toVector == Vector3.zero)       // is enemy move 
    //    {
    //        return 0f;                      // yes then return 0
    //    }

    //    float angle = Vector3.Angle(fromVector, toVector);
    //    Vector3 normal = Vector3.Cross(fromVector, toVector);       // is he right or left
    //    angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
    //    angle *= Mathf.Deg2Rad;
    //    print("angle " + angle);
    //    return angle;
    //}



}
