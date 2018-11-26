using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharMovement : MonoBehaviour
{
    private CapsuleCollider capcollider;
    public float volumen;
    public AudioClip CrouchSteps;
    public AudioClip NormalStep;
    public AudioClip LaborStep;

    float countdown;
    float CountdownFootStopes;

    public float CountdownFootStepSprint;       // abfrage genug zeit vergangen seit letztem footstep?
    public float CountdownFootStep;
    public AudioClip CurrentClip;

    float MouseX;
    
    Animator CharAni;
    bool running = false;

    public bool StopMovement;


    void Start()
    {
        capcollider = GetComponent<CapsuleCollider>();
        CharAni = GetComponent<Animator>();
        CurrentClip = LaborStep;
    }


    private void Update()
    {
        MouseX = Input.GetAxis("Mouse X"); // deaktiviert bei bewegung die drehung 
        Countdown();
        CountdownFootStopes += Time.deltaTime;

            RunningFunction();
            CrouchFunction();
            JumpFunction();
            InputControlls();
            MouseTurn();

    }



    void MouseTurn()
    {
        if (MouseX < -0.3 || MouseX > 0.3 )
        {
            CharAni.SetBool("OutTurn",true);
        }
        else
        {
            CharAni.SetBool("OutTurn", false);
        }
    }

    void InputControlls()
    {
        if (Input.anyKeyDown)
        {
            CharAni.SetTrigger("OutTurn");
        }
    }


    void RunningFunction()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            CharAni.SetBool("Sprint", true);
            running = true;
        }
        else
        {
            CharAni.SetBool("Sprint", false);
            running = false;
        }
    }
    void CrouchFunction()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            CharAni.SetBool("Crouch", true);
            capcollider.height = 1.24f;
            capcollider.center = new Vector3(0, 0.61f, 0);
        }
        else
        {
            CharAni.SetBool("Crouch", false);
            capcollider.height = 1.89f;
            capcollider.center = new Vector3(0,0.92f,0);

        }
    }

    void JumpFunction()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (running)
            {
                CharAni.SetBool("Jump", true);
            }
            else
            {
                CharAni.SetBool("WalkToJump", true);
            }
        }
    }
    void Countdown()
    {
        if (Input.anyKey)
        {
            countdown = 0;
        }
        else
        {
            countdown += Time.deltaTime;
        }
    }



    void StepSound()            // wird von animation event ausgelöst 
    {
        if (Input.anyKey || MouseX < -0.3 || MouseX > 0.3 )
        {
            if (CountdownFootStopes >= CountdownFootStep)
            {
                AudioSource.PlayClipAtPoint(CurrentClip, transform.position, volumen);
                CountdownFootStopes = 0;
            }
        }
        else if (countdown <= 0.4f)             // verzögerung des letzten schrittes 
        {
            if (CountdownFootStopes >= CountdownFootStep)
            {
                AudioSource.PlayClipAtPoint(CurrentClip, transform.position, volumen);
                CountdownFootStopes = 0;
            }
        }

    }

    void RunSound()
    {
        if (Input.anyKey || MouseX < -0.3 || MouseX > 0.3)
        {
            if (CountdownFootStopes >= CountdownFootStepSprint)
            {
                AudioSource.PlayClipAtPoint(CurrentClip, transform.position, volumen);
                CountdownFootStopes = 0;
            }
        }
        else if (countdown <= 0.3f)             // verzögerung des letzten schrittes
        {
            if (CountdownFootStopes >= CountdownFootStepSprint)
            {
                AudioSource.PlayClipAtPoint(CurrentClip, transform.position, volumen);
                CountdownFootStopes = 0;
            }
        }
    }  

    void StepSoundCrouch()            // wird von animation (nicht animator!) event ausgelöst 
    {
        if (Input.anyKey || MouseX < -0.3 || MouseX > 0.3)
        {
            AudioSource.PlayClipAtPoint(CrouchSteps, transform.position, volumen);
            CountdownFootStopes = 0;
        }
        else if (countdown <= 0.3f)             // verzögerung des letzten schrittes
        {
            AudioSource.PlayClipAtPoint(CrouchSteps, transform.position, volumen);
            CountdownFootStopes = 0;
        }

    }

}