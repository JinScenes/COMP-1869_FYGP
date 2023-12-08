using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;



public class PlayerStats
{
    public Inventory inventory;
    public PlayerAmmo playerAmmo;
    
    public static int currency;
    public float speed;
   
    public InventoryUI UIHandle;

    public PlayerStats(int playerIndex, ItemData weightItem, gunHolder holder, GunBase gunBase, Consumables consumables)
    {
        UIHandle = new InventoryUI(playerIndex);
        inventory = new Inventory(UIHandle, weightItem, holder, gunBase, consumables);
        playerAmmo = new PlayerAmmo(UIHandle);
    }

    public void StartEvent(MonoBehaviour myMonoBehaviour, IEnumerator coroutine)
    {
        //Setup event parameters
        myMonoBehaviour.StartCoroutine(coroutine);
    }


}

public class Ammo
{
    private PlayerAmmo playerAmmo;

    private int Ammount = 3000;
    public int ammount { 
        get { return Ammount; } 
        set {
            //Debug.Log("Trying to change ammo amount");
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
