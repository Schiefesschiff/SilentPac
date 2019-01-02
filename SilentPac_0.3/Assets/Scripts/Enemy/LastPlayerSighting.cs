using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastPlayerSighting : MonoBehaviour
{
    public Vector3 position = new Vector3(1000f, 1000f, 1000f);
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
    public float lightHighIntensity = 0.25f;
    public float lightLowIntensity = 0f;
    public float fadeSpeed = 7f;
    public float musicFadeSpeed = 1f;
    public  int delayAlarmTimer = 2;
    private float timeLeft;

    private CameraController camCon;
    private AlarmLight alarmLight;
    private Light mainLight;
    private AudioSource normalAudio;
    private AudioSource panicAudio;
    private AudioSource[] sirens;
    public GameObject[] forks;
    private PlayerController playerController;
    private GameObject player;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        normalAudio = GetComponent<AudioSource>();
        panicAudio = transform.Find("secondaryMusic").GetComponent<AudioSource>();
        alarmLight = GameObject.FindGameObjectWithTag("AlarmLight").GetComponent<AlarmLight>();
        mainLight = GameObject.FindGameObjectWithTag("MainLight").GetComponent<Light>();
        camCon = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        timeLeft = delayAlarmTimer;

        GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag("Siren");
        sirens = new AudioSource[sirenGameObjects.Length];

        for (int i = 0; i < sirens.Length; i++)     // added all finding sirens in array
        {
            sirens[i] = sirenGameObjects[i].GetComponent<AudioSource>();
        }

        forks = GameObject.FindGameObjectsWithTag("Forks");
    }

    private void Update()
    {
        SwitchAlarms();
        MusicFading();

        if (Input.GetKey(","))
        {
            lightHighIntensity += 0.1f;
        }
        if (Input.GetKey("."))
        {
            lightHighIntensity -= 0.1f;
        }

        if (playerController.pullEnergy)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                position = player.transform.position;
            }
        }
        else
        {
            timeLeft = delayAlarmTimer;
        }
    }




    void SwitchAlarms()
    {
        alarmLight.alarmOn = (position != resetPosition);
        camCon.Alarm = (position != resetPosition);

        float newIntensity;

        if (position != resetPosition)
        {
            newIntensity = lightLowIntensity;
        }
        else
        {
            newIntensity = lightHighIntensity;
        }

        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        for (int i = 0; i < sirens.Length; i++)
        {
            if (position != resetPosition && !sirens[i].isPlaying)
            {
                sirens[i].Play();
            }
            else if (position == resetPosition)
            {
                sirens[i].Stop();
            }
        }
    }

    void MusicFading()      //      music/alarmSound fading in and out with controlling volumen
    {
        if (position != resetPosition)
        {
            normalAudio.volume = Mathf.Lerp(normalAudio.volume, 0, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
        }
        else
        {
            normalAudio.volume = Mathf.Lerp(normalAudio.volume, 0.8f, musicFadeSpeed * Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f, musicFadeSpeed * Time.deltaTime);
        }
    }

}
