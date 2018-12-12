using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventController
{
    public static System.Action<EnemyAI> EnemySpawned;
    public static System.Action<EnemyAI> EnemyRemoved;

    //public static System.Action Hurt;
}
