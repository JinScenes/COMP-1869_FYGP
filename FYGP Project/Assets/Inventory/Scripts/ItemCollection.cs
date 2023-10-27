using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollection : MonoBehaviour, ICollectable
{
    public ItemData item;
    [SerializeField] bool isGun = false;
    private GunDataTransmiitter GDT;
    private GunHolder gunholder;
    private GunData gun;
    private bool collected;
    private bool equppefull;

    private void Start()
    {
        if(isGun == true)
        {
            gunholder = gameObject.GetComponent<GunHolder>();
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

            if (isGun)
            {
                // Simply add the gun to the player's inventory.
                playerStats.inventory.Add(gun);
                Destroy(gameObject);
            }
            else
            {
                // Handle collection of other item types if needed.
                // For now, I'm leaving it blank.
            }
        }
    }
}
