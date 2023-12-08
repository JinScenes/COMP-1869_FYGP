using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //playerStats = GetComponentInParent<PlayerStats>();
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
        if (CurrentGunData != null)  
        {


            PreviousGunData = CurrentGunData;
            CurrentGunData = newGunData;
            Destroy(CurrentGunGameObject);
            gunBase.Initialize(CurrentGunData);
            GameObject gunFloorLootPrefab = Resources.Load<GameObject>("GunFloorLoot");
            GameObject spawnedLoot = Instantiate(gunFloorLootPrefab, new Vector3(transform.position.x, transform.position.y+ 3, transform.position.z ), Quaternion.identity);
            Rigidbody rb = spawnedLoot.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 shootDirection = Vector3.up;  // Adjust for desired direction (more upward force for a higher arc)
                rb.AddForce(shootDirection * 3f, ForceMode.Impulse);
            }
            // Get the ItemCollection component and set its item to the previous gun data
            ItemCollection itemCollection = spawnedLoot.GetComponent<ItemCollection>();
            if (itemCollection != null)
            {
                itemCollection.item = PreviousGunData;
            }
            else
            {
                Debug.LogError("GunFloorLoot prefab does not have ItemCollection script attached!");
            }
            
            //playerStats.inventory.Add(PreviousGunData as ItemData);
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

