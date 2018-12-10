using UnityEngine;
using UnityEngine.UI;

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

    private bool healthWippeDown = true; //Testfunktion

    [Header("Inventory")]
    public Image keyImage;
    public Image fuseImage;

    void Start()
    {
        keyImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
        fuseImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
    }

    void Update()
    {
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
        
        healthBar.fillAmount = health / maxHealth;
        energyBar.fillAmount = energy / maxEnergy;
    }

    public void addKeyToInventoryUI()
    {
        keyImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
    }
    
    public void removeKeyFromInventoryUI()
    {
        keyImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
    }

    public void addFuseToInventoryUI()
    {
        fuseImage.GetComponent<Image>().color = new Color32(255, 255, 225, 100);
    }

    public void removeFuseFromInventoryUI()
    {
        fuseImage.GetComponent<Image>().color = new Color32(100, 100, 100, 100);
    }


}
