using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour, ICollectable
{
    public ItemData item;

    private bool collected;
    private bool equppefull;


    private void Start()
    {
        if(isGun == true)
        {
            //gunholder = gameObject.GetComponent<GunHolder>();
            GDT = gameObject.GetComponent<GunDataTransmiitter>();
            gun = GDT.HeldGun;
        }
    }
    public void Collect(object plrStats)
    {
        if (!collected)
        {
            collected = true;
            PlayerStats playerStats = plrStats as PlayerStats;
            
            if (playerStats.inventory.Add(item))
            {
                Destroy(gameObject);
            } else {
                Debug.Log("Player could not pick up so not removed!");
                collected = false;
            }
            
        }
        
    }
}
