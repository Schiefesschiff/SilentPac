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

    //private bool healthWippeDown = true; //Testfunktion

    [Header("Inventory")]
    public Image keyImage;
    public Image fuseImage;

    [Header("ButtonDiamond")]
    public Image buttonImage_A;
    public Image buttonImage_B;
    public Image buttonImage_X;
    public Image buttonImage_Y;

    private PlayerEnergy playerEnergy;

    private void Start()
    {
        playerEnergy = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEnergy>();
        keyImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        fuseImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        MakeButtonDark(buttonImage_Y);
        MakeButtonDark(buttonImage_A);

        health = maxHealth;
        energy = maxEnergy;
    }

    private void FixedUpdate()
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

        if (playerEnergy.currentStanima >= 5f)        //makes B and X buttons bright when player has enough energy

        {
            MakeButtonBright(buttonImage_B);
            MakeButtonBright(buttonImage_X);
        }
        else
        {
            MakeButtonDark(buttonImage_B);
            MakeButtonDark(buttonImage_X);
        }

        
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health, 2f * Time.deltaTime);
        energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, energy, 2f* Time.deltaTime);
    }

    public void ReduceHealth(float _health , float _energy)
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
        buttonImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);//soll hier wirklich die alpha runter?
    }

    public void MakeButtonBright(Image buttonImage)
    {
        buttonImage.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }
    
}
