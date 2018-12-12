using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [Header("Health")]
    private float maxHealth = 100;
    private float health;
    public Image healthBar;

    [Header("Energy")]
    private float maxEnergy = 100;
    private float energy;
    public Image energyBar;
    //public TextMeshProUGUI myTextMeshProGui;

    private bool healthWippeDown = true; //Testfunktion

    [Header("Inventory")]
    public Image keyImage;
    public Image fuseImage;

    [Header("ButtonDiamond")]
    public Image buttonImage_A;
    public Image buttonImage_B;
    public Image buttonImage_X;
    public Image buttonImage_Y;

    void Start()
    {
        keyImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        fuseImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        MakeButtonDark(buttonImage_Y);
        MakeButtonDark(buttonImage_A);

        health = maxHealth;
        energy = maxEnergy;
    }

    void Update()
    {
        //need to get health and energy from the playerController here

        //Testfunktion, lässt Health hoch und runter gehen
        //if (health <= 0f)
        //    healthWippeDown = false;
        //if (health >= 100f)
        //    healthWippeDown = true;

        //if (healthWippeDown)
        //    health --;
        //else
        //    health ++;

        //energy = health; //Testfunktion Ende

        //if (energy >= 20f)
        //{
        //    MakeButtonBright(buttonImage_B);
        //    MakeButtonBright(buttonImage_X);
        //}
        //else
        //{
        //    MakeButtonDark(buttonImage_B);
        //    MakeButtonDark(buttonImage_X);
        //}
        healthBar.fillAmount = Mathf.Lerp(1, health, 0.5f);
        energyBar.fillAmount = Mathf.Lerp(1, energy, 0.5f);
    }

    public void ReduceHealth(int _health , int _energy)
    {
        health = _health / maxHealth;
        energy = _energy / maxEnergy;
    }



    public void AddKeyToInventoryUI()
    {
        keyImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    
    public void RemoveKeyFromInventoryUI()
    {
        keyImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
    }

    public void AddFuseToInventoryUI()
    {
        fuseImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void RemoveFuseFromInventoryUI()
    {
        fuseImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
    }

    public void MakeButtonDark(Image buttonImage)
    {
        buttonImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
    }

    public void MakeButtonBright(Image buttonImage)
    {
        buttonImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }


}
