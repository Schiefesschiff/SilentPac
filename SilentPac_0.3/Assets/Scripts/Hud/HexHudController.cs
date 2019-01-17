using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //evtl obsolet

public class HexHudController : MonoBehaviour
{
    [Header("Energy")]
    private float maxEnergy = 100;
    private float energy;
    private PlayerEnergy playerEnergy;
    public Image energyBar;
    
    //[Header("LayerHex")]

    //[Header("ButtonHex")]

    private void Start()
    {
        playerEnergy = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEnergy>();
        
        energy = maxEnergy;
    }

    private void Update()
    {/*
        if (ALARM)
            alarmTimer = ALARMTIME;
     */
    }

    //Methods for LayerHex
    private void ScaleLayer(int layer = 0)
    {

    }

    private void MakeLayerWhite(int layer = 0)
    {

    }

    private void MakeLayerGrey(int layer = 0)
    {

    }

    private void MakeLayerBlue(int layer = 0)
    {

    }
    
    //Methods for Bar
    public void MakeBarWhite()
    {

    }

    private void MakeBarOrange()
    {

    }

    private void SetRedBarWidth(float newWidth)
    {

    }

    //Methods for ButtonHex
    public void SwitchButtonShown(string newButton)
    {
        switch (newButton.ToLower()) //ToLower() makes the switch case-insensitive
        {
            case "a":
                //show A
                break;

            case "rb":
                //show RB
                break;

            case "rt":
                //show RT
                break;

            default:
                //showNoButton
                break;
        }
    }

    public string WhichButtonToShow()
    {
        //is there a dialogue going on?
        //am i still on a plattform?
        //do i have enough energy to shoot?
        
        return ("Could not decide which button to show.");
    }
}
