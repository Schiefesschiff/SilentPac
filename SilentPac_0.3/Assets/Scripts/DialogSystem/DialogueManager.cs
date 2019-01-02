using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    public Animator animator;

    public AudioSource audioSource;
    public AudioClip[] Clicks;
    private AudioClip ClickClip;
    public AudioClip[] SpacesClip;
    private AudioClip SpaceClip;

    private Queue<string> sentences;        // spezial array
    private float startVolumen;

    void Start()
    {
        sentences = new Queue<string>();
        startVolumen = audioSource.volume;
    }

    private void Update()
    {
        if (Input.GetButtonDown(StringCollection.INPUT_A))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue )
    {
        audioSource.volume = startVolumen;

        animator.SetBool("IsOpen", true);
        audioSource = GetComponent<AudioSource>();

        nameText.text = dialogue.name;      // name from NPC

        sentences.Clear();                  // clear string array 

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);        // addin to spezial array
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()       // next DialogTextbox
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            audioSource.volume = 0;
            return;
        }
        string sentence = sentences.Dequeue();      // give one element 
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence)); // start Coroutine for output
    }

    IEnumerator TypeSentence (string sentence)      // output one Letter from String with delay
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            if (letter.ToString() != " ")           // if press SpaceKey play other Sound
            {
                RandomClickSound();
            }
            else
            {
                //RandomSpaceSound();        // play SpaceClip
            }
            yield return new WaitForSeconds(Random.Range(0.12f,0.09f));
        }

    }

    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

    void RandomClickSound()
    {
        int index = Random.Range(0, Clicks.Length);
        ClickClip = Clicks[index];
        audioSource.PlayOneShot(ClickClip);     // play at same time more sound at one audiosources
    }

    void RandomSpaceSound()
    {
        int index = Random.Range(0, SpacesClip.Length);
        SpaceClip = SpacesClip[index];
        audioSource.PlayOneShot(SpaceClip);     // play at same time more sound at one audiosources
    }
}
