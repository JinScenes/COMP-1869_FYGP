using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour, ICollectable
{
    public ItemData item;
    public bool collected;

    public void Collect(object plrStats)
    {
        if (!collected)
        {
            collected = true;
            //print("Locally heard the collect");
            PlayerStats playerStats = plrStats as PlayerStats;
            if (playerStats.inventory.Add(item))
            {
                Destroy(gameObject);
            } else
            {
                Debug.Log("Player could not pick up so not removed!");
                collected = false;
            }
            
        }
     
    }
}
