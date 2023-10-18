using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerStats
{
    public Inventory inventory;
    public PlayerAmmo playerAmmo;
  
    public int currency;
    public float health;
    public float speed;

    public InventoryUI UIHandle;

    public PlayerStats(int playerIndex)
    {
        UIHandle = new InventoryUI(playerIndex);
        inventory = new Inventory(UIHandle);
        playerAmmo = new PlayerAmmo(UIHandle);
    }
}

public class Ammo
{
    private PlayerAmmo playerAmmo;

    private int Ammount;
    public int ammount { 
        get { return Ammount; } 
        set {
            Debug.Log("Trying to change ammo amount");
            Ammount = value;
            playerAmmo.UIHandle.UpdateAllAmmo(playerAmmo);
        } 
    }

    public Ammo(PlayerAmmo UIHandle)
    {
        playerAmmo = UIHandle;
    }
}

public class PlayerAmmo 
{

    public InventoryUI UIHandle;

    public Ammo smallAmmo;
    public Ammo mediumAmmo;
    public Ammo largeAmmo;

    public PlayerAmmo(InventoryUI UIHandle)
    {
        this.UIHandle = UIHandle;

        smallAmmo = new Ammo(this);
        mediumAmmo = new Ammo(this);
        largeAmmo = new Ammo(this);
    }

}
