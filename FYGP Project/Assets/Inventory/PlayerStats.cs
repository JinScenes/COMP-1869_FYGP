using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats
{
    /* private int playerIndex;
     public PlayerStats(int playerIndex)
     {
         playerIndex = this.playerIndex;
     }*/
    public Inventory inventory = new Inventory();
    public PlayerAmmo playerAmmo = new PlayerAmmo();
  
    public int currency;
    public float health;
    public float speed;



}

public class AmmoType
{
    public int ammo;
}

public class S_Ammo : AmmoType
{
}

public class M_Ammo : AmmoType
{
}

public class L_Ammo : AmmoType
{
}

public class PlayerAmmo 
{
    public S_Ammo smallAmmo;
    public M_Ammo mediumAmmo;
    public L_Ammo largeAmmo;
}


[CreateAssetMenu]
public class Gun : ItemData
{
    public int coolthing;
}