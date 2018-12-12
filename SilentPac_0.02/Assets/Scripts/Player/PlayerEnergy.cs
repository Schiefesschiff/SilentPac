using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth { get ; private set;}      // read from outside change only from this script

    public int maxStanima = 100;
    public int currentStanima { get; private set; }

    private HudController hud;
    private int reduceValueStamina;
    private Animator ani;
    private void Awake()
    {
        hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>();
        ani = GetComponent<Animator>();
        currentHealth = maxHealth;
        currentStanima = maxStanima;
    }

    private float reduceTimer;

    private void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            TakeDamage(30);
        }


        if (reduceValueStamina > 0)
        {
            reduceTimer += Time.deltaTime;
            if (reduceTimer > 0.2f)
            {
                reduceStamina(reduceValueStamina);
                reduceTimer = 0;
            }
        }
    }




    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        hud.ReduceHealth(currentHealth, currentStanima);

        Debug.Log(transform.name + " take " + damage + " damage ");

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public bool AddStamina(int value)
    {
        currentStanima += value;
        if (currentStanima >= maxStanima)
        {
            currentStanima = maxStanima;
            return false;
        }
        return true; 
    }

    public void StopConsume(int value)
    {
        if (reduceValueStamina <= 0)
        {
            reduceValueStamina = 0;
        }
        else
        {
            reduceValueStamina -= value;
        }
    }

    public bool UseStanima(int use)
    {
        reduceValueStamina += use;

        if (reduceValueStamina <= currentStanima)
        {

            //print("genug energy " + currentStanima);
            return true;
        }
        reduceValueStamina -= use;
        return false;

    }

    void reduceStamina(int s)
    {
        currentStanima -= s;
    }


    public virtual void Die()
    {
        // die in same way
        // this mothod is meant to be overwritteb
        Debug.Log(transform.name + " died");
        ani.SetBool("isDying", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ElectricRiotStick")
        {
            Debug.Log("entered");
            TakeDamage(30);
        }
    }

}
