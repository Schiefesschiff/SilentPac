using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private EnemyAI enemyAI;
    public int health;
    public int maxHeatl = 100;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        health = maxHeatl;
    }


    public void GotDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            enemyAI.isDeath = true;
        }
    }
}
