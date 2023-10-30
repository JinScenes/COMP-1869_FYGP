using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class gunHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public GunData CurrentGunData;
    public GunData PreviousGunData;  // Added this to hold the previous gun data
    public GameObject CurrentGunGameObject;
    public Inventory inventory;
    public GunBase gunBase;
    // reference to the current gun game object
    public PlayerStats playerStats;
    void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
    }
    /*public void EquipGun(GunData newGunData)
    {
        if (CurrentGunData != null)
        {
            inventory.Add(CurrentGunData);  // Return the old gun to the inventory
        }

        CurrentGunData = newGunData;  // Update the current gun data

        if (CurrentGunGameObject != null)
        {
            Destroy(CurrentGunGameObject);  // Destroy the old gun game object
        }

        if (newGunData != null)
        {
            CurrentGunGameObject = Instantiate(newGunData.gunModel, transform.position, transform.rotation, transform);  // Instantiate the new gun model
        }
    }*/

    public void SwapGun(GunData newGunData)
    {
        if (CurrentGunData != null)  // Handling the first gun being consumed
        {


            PreviousGunData = CurrentGunData;
            CurrentGunData = newGunData;
            Destroy(CurrentGunGameObject);
            gunBase.Initialize(CurrentGunData);
            playerStats.inventory.Add(PreviousGunData as ItemData);
            CurrentGunGameObject = gunBase.instGun;
            //inventory.Add(PreviousGunData as ItemData);  // Return the old gun to the inventory



            CurrentGunGameObject = gunBase.instGun;
            // Destroy the old gun game object
        }

          // Update the current gun data

        if (CurrentGunData == null)
        {
            CurrentGunData = newGunData;
            gunBase.Initialize(CurrentGunData);
            
            CurrentGunGameObject = gunBase.instGun;  // Instantiate the new gun model
        }
    }


    public void UpdateGunData(GunData newGunData)
    {
        CurrentGunData = newGunData;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

