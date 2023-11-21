using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCollection : MonoBehaviour, ICollectable
{

    public LootAmmo ammo;
    public int amount = 5;

    private bool collected;

    public void Collect(object plrStats)
    {
        if (!collected)
        {
            collected = true;
            PlayerStats playerStats = plrStats as PlayerStats;

            if (ammo.GetType() == typeof(LootAmmo))
            {
                LootAmmo item = (LootAmmo)this.ammo;
                switch (item.ammoType)
                {
                    case AmmoType.SmallAmmo:
                        playerStats.playerAmmo.smallAmmo.ammount += amount;
                        break;
                    case AmmoType.MediumAmmo:
                        playerStats.playerAmmo.mediumAmmo.ammount += amount;
                        break;
                    case AmmoType.LargeAmmo:
                        playerStats.playerAmmo.largeAmmo.ammount += amount;
                        break;
                    default:
                        break;
                }


                print($"Added to player {item.ammoType} + {amount}!");
            }

            Destroy(gameObject);
        }

    }
}
