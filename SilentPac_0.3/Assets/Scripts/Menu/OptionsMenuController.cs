using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetBrightness(float newBrightness)
    {

    }

    public void SetVolume(float volume)
    {
        Debug.Log("Setting Volume to " + volume + ".");
        audioMixer.SetFloat("volume", volume);
    }

}
