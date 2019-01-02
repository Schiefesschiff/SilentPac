using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour
{
    List<EnemyAI> enemies = new List<EnemyAI>();

    bool hurtConnected;

    void Awake()
    {
        WorldEventController.EnemySpawned += AddEnemyToWatchList;
        WorldEventController.EnemyRemoved += RemoveEnemyFromWatchList;
    }
    //GameObject[] enemies

    void AddEnemyToWatchList(EnemyAI enemy)
    {
        enemies.Add(enemy);
        //Pfeil spawnen (an/aus nach reichweite woanders)

    }

    void RemoveEnemyFromWatchList(EnemyAI enemy)
    {
        enemies.Remove(enemy);
        //Pfeil despawnen
    }

    /*
    void HurtConnect(bool attached)
    {
        if (hurtConnected == attached) return;
        if (attached)
            WorldEventController.Hurt += Aua;
        else
            WorldEventController.Hurt -= Aua;
        hurtConnected = attached;
    }

    void Aua()
    {
    }
    */
    void OnDestroy()
    {
        WorldEventController.EnemySpawned -= AddEnemyToWatchList;
        WorldEventController.EnemyRemoved -= RemoveEnemyFromWatchList;
    }
}
