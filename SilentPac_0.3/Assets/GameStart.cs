using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    //private Animator HUDAni;
    public Dialogue startDialogue;

    // Start is called before the first frame update
    void Start()
    {
        //HUDAni = GameObject.FindGameObjectWithTag("HUD").GetComponent<Animator>();
        StartCoroutine(StartDialog());
    }

    IEnumerator StartDialog()
    {
        yield return new WaitForSeconds(2);
        FindObjectOfType<DialogueManager>().StartDialogue(startDialogue);
    }
}
