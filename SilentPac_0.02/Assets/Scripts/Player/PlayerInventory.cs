using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool hasKey;
    public bool hasFuse;

    void Awake()
    {
        hasKey = false;
        hasFuse = false;
    }

    
}
