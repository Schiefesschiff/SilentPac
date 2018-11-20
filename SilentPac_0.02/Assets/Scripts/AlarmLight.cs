using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    public float fadeSpeed = 2f;
    public float highIntensity = 2f;
    public float lowIntensity = 0.5f;
    public float changeMargin = 0.2f;
    public bool alarmOn;

    private Light thisLight;
    private float targetIntensity;

    private void Awake()
    {
        thisLight = GetComponent<Light>();
        GetComponent<Light>().intensity = 0f;
        targetIntensity = highIntensity;
    }

    private void Update()
    {
        if (alarmOn)        // if alarm on change Intensity
        {
            thisLight.intensity = Mathf.Lerp(thisLight.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            CheckTargetIntensity();
        }
        else
        {
            thisLight.intensity = Mathf.Lerp(thisLight.intensity, 0, fadeSpeed * Time.deltaTime);
        }
    }

    void CheckTargetIntensity()     // produce a wave on light
    {
        if (Mathf.Abs(targetIntensity - thisLight.intensity) < changeMargin)
        {
            if (targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }
            else
            {
                targetIntensity = highIntensity;
            }
        }
    }

}
