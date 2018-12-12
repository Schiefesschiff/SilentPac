using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudController : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100;
    public float health = 100;
    public Image healthBar;

    [Header("Energy")]
    public float maxEnergy = 100;
    public float energy = 100;
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
    }

    void Update()
    {
        //need to get health and energy from the playerController here

        //Testfunktion, lässt Health hoch und runter gehen
        if (health <= 0f)
            healthWippeDown = false;
        if (health >= 100f)
            healthWippeDown = true;

        if (healthWippeDown)
            health --;
        else
            health ++;
        
        energy = health; //Testfunktion Ende

        if (energy >= 20f)
        {
            MakeButtonBright(buttonImage_B);
            MakeButtonBright(buttonImage_X);
        }
        else
        {
            MakeButtonDark(buttonImage_B);
            MakeButtonDark(buttonImage_X);
        }
        
        healthBar.fillAmount = health / maxHealth;
        energyBar.fillAmount = energy / maxEnergy;
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
