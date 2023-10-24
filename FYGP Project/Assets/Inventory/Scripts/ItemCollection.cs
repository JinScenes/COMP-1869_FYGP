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





            if (!isGun)
            {
                if (playerStats.inventory.Add(item))
                {
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Player could not pick up so not removed!");
                    collected = false;
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGun)
        {
            if (other.GetComponent<GunHolder>().equippedGun == null)
            {
                other.GetComponent<GunHolder>().equippedGun = gun;
            }
        }
        
    }
}
