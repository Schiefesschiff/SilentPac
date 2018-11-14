using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectingScript : MonoBehaviour
{
    public GameObject enemy;
    private EnemyScript enemyScript;

    private void Awake()
    {
        enemyScript = enemy.GetComponent<EnemyScript>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            enemyScript.NewTarget();
        }
    }

}
