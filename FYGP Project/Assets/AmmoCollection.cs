using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollection : MonoBehaviour, ICollectable
{

    public ItemData item;
    public bool collected;

    public void Collect(object plrStats)
    {
        if (!collected)
        {
            collected = true;
            PlayerStats playerStats = plrStats as PlayerStats;

            if (item.GetType() == typeof(S_AmmoLoot))
            {
                S_AmmoLoot ammo = (S_AmmoLoot) item;

                playerStats.playerAmmo.smallAmmo.ammo += Random.Range(ammo.ammountStack, ammo.ammountStack + 10);
                print("SMALL AMMO AMT: " + playerStats.playerAmmo.smallAmmo.ammo);
            } 
            else if (item.GetType() == typeof(M_AmmoLoot))
            {
                M_AmmoLoot ammo = (M_AmmoLoot)item;

                playerStats.playerAmmo.mediumAmmo.ammo += Random.Range(ammo.ammountStack, ammo.ammountStack + 10);
                print("SMALL MID AMT: " + playerStats.playerAmmo.mediumAmmo.ammo);
            }
            else if (item.GetType() == typeof(L_AmmoLoot))
            {
                L_AmmoLoot ammo = (L_AmmoLoot)item;

                playerStats.playerAmmo.largeAmmo.ammo += Random.Range(ammo.ammountStack, ammo.ammountStack + 10);
                print("LARGE AMMO AMT: " + playerStats.playerAmmo.largeAmmo.ammo);
            }
                Destroy(gameObject);
        }

    }
}
