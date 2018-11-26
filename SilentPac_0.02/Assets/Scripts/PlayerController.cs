using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;


public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent nav;

    public ThirdPersonCharacter character;

    [Range(0.5f, 1f)] [SerializeField] float speed = 0.5f;

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                nav.SetDestination(hit.point);
            }
        }

        if (nav.remainingDistance > nav.stoppingDistance)       // distance between enemy / player
        {
            character.Move(nav.desiredVelocity, false, false, speed);      // for third person controller(animation)
        }
        else
        {
            character.Move(Vector3.zero, false, false, speed);
        }

    }
}
