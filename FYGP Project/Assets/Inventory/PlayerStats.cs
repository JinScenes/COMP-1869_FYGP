using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public S_Ammo smallAmmo = new S_Ammo();
    public M_Ammo mediumAmmo = new M_Ammo();
    public L_Ammo largeAmmo = new L_Ammo();
}

[CreateAssetMenu(menuName ="SmallAmmoLoot")]
public class S_AmmoLoot : ItemData
{
    public int ammountStack = 5;
}

[CreateAssetMenu(menuName = "MediumAmmoLoot")]
public class M_AmmoLoot : ItemData
{
    public int ammountStack = 5;
}

[CreateAssetMenu(menuName = "LargeAmmoLoot")]
public class L_AmmoLoot : ItemData
{
    public int ammountStack = 5;
}

[CreateAssetMenu]
public class Gun : ItemData
{
    public int coolthing;
}