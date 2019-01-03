using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class OptionsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public PostProcessVolume postProcessVolume;
    public ColorGrading colorGrading;
    private Vector4 newGammaVector;

    private void Start()
    {
        postProcessVolume = GameObject.FindGameObjectWithTag("GameController").GetComponent<PostProcessVolume>();

        if (postProcessVolume.sharedProfile == null)     //Testet ob ein PostProcessVolume gefunden wurde.
        {
            enabled = false;
            Debug.Log("Can't load PostProcessVolume.");
            return;
        }
        
        bool foundEffectSettings = postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out colorGrading);     //Testet ob die colorGrading Settings angesprochen werden können.
        if (!foundEffectSettings)
        {
            enabled = false;
            Debug.Log("Can't load PitchTest settings.");
            return;
        }        
    }
    
    public void SetGamma(float newGamma)    //Setzt den PostProcessing-Gamma.
    {
        if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out colorGrading))
        {
            Debug.Log("Got gamma value: " + colorGrading.gamma.value);
            newGammaVector = new Vector4 (0, 0, 0, newGamma);
            colorGrading.gamma.value = newGammaVector;                
            Debug.Log("Setting value to: " + newGammaVector);            
        }
    }

    public void SetContrast(float newContrast)  //Setzt den PostProcessing-Contrast.
    {
        if (postProcessVolume.sharedProfile.TryGetSettings<ColorGrading>(out colorGrading))
        {
            //colorGrading.contrast.SetValue(new FloatParameter() { value = newContrast });
            Debug.Log("Setting Contrast to: " + newContrast);
            colorGrading.contrast.value = newContrast;
        }
    }

    public void SetVolume(float volume)     //Setzt Volume vom MainMixer.
    {
        Debug.Log("Setting Volume to " + volume + ".");
        audioMixer.SetFloat("volume", volume);
    }
}