using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.InputSystem.Controls;
using System.Xml;
using Unity.VisualScripting.FullSerializer;
using static UnityEditor.Progress;

[System.Serializable]
public class Inventory
{
    private int maxInventorySlots = 4;
    private int maxStack = 4;
    private ItemData weightItem;
    public InventoryItem[] inventory = new InventoryItem[4];
    private Dictionary<int, InventoryItem> itemDictionary = new Dictionary<int, InventoryItem>();

    private InventoryUI inventoryUI;

    public Inventory(InventoryUI inventoryUI, ItemData weightItem)
    {
        this.inventoryUI = inventoryUI;
        this.weightItem = weightItem;
    }
    
    public int GetCount()
    {
        int objCount = 0;
        foreach (InventoryItem go in inventory)
        {
            if (go != null)
            {
                objCount++;
            }
        }

        return objCount;

    }

    public int FindFreeIndex()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                return i;
            }
        }

        Debug.LogError("Could not find a free index in inventory!");
        return 6;
    }
    private bool isItemAGun(ItemData itemData)
    {
        return itemData.GetType() == typeof(GunData);
    }
    private void AddWeight(InventoryItem gunInventoryItem)
    {
        Debug.Log("Adding gun weight");
        ItemData weight = weightItem;

        InventoryItem newItem = new InventoryItem(weight);

        int newIndex = FindFreeIndex();
        gunInventoryItem.weightIndex = newIndex;
        inventory[newIndex] = newItem;

        Debug.Log("Set weight index to " + newIndex);

        itemDictionary.Add(newIndex, newItem);
    }

    private bool AddItem(ItemData itemData)
    {
        bool isAGun = isItemAGun(itemData);

        if (GetCount() + (isAGun ? 1 : 0) < maxInventorySlots)
        {
            InventoryItem newItem = new InventoryItem(itemData);

            int newIndex = FindFreeIndex();
            inventory[newIndex] = newItem;
            itemDictionary.Add(newIndex, newItem);

            if (isAGun){
                AddWeight(newItem);
            }

            //Debug.Log($"{itemData.displayName} has now been added to the inventory");
            inventoryUI.UpdateAll(this);
            return true;
        } else
        {
            Debug.Log($"Could not add item {itemData.displayName} to inventory bc it is full!");
            return false;
        }

    }

    private InventoryItem FindItem(ItemData itemData)
    {
        foreach (KeyValuePair<int, InventoryItem> item in itemDictionary)
        {
            if(item.Value.itemData == itemData)
            {
                //Debug.Log($"Found {item.Value.itemData.displayName} in inventory!");
                return item.Value;
            }
            //Debug.Log("This");
        }

        //Debug.Log($"Did not find {itemData.displayName} in inventory!");

        return null;
    }

    private InventoryItem FindItemThatsStackable(ItemData itemData)
    {
        foreach (KeyValuePair<int, InventoryItem> item in itemDictionary)
        {
            InventoryItem itemValue = item.Value;

            if (itemValue.itemData == itemData)
            {
                //Debug.Log($"Found {item.Value.itemData.displayName} in inventory!");
                if(itemValue.stackSize < maxStack)
                {
                    //Debug.Log($"Found {itemValue.itemData.displayName} in inventory and its stackable! ({itemValue.stackSize})");
                    return itemValue;
                }
                else
                {
                    continue;
                }
               
            }
            //Debug.Log("This");
        }

        //Debug.Log($"Did not find a stackable {itemData.displayName} in inventory, returning null");

        return null;
    }

    public bool Add(ItemData itemData)
    {
        // If Item is in inventory
        InventoryItem item = FindItem(itemData);
        if (item != null)
        {
            // If item can stack, add a stack
            if (item.itemData.canStack)
            {
                if (item.stackSize >= maxStack)
                {
                    InventoryItem stackableItem = FindItemThatsStackable(itemData);
                    if(stackableItem != null)
                    {
                        stackableItem.AddToStack();
                        inventoryUI.UpdateAll(this);
                        return true;
                    }
                    else
                    {
                        return AddItem(itemData);
                    }
                    
                }
                else
                {
                    item.AddToStack();
                    //Debug.Log($"INVENTORY : {itemData.displayName} total stack is now {item.stackSize}");
                    inventoryUI.UpdateAll(this);
                    return true;
                }
              

            // if an item can't stack, attempt to add
            } else  {
                //Debug.Log("Item can't stack, attempt to create new instance");
                return AddItem(itemData);
            }
        // Add Item to inventory bc it is not in inventory
        } else {
          return AddItem(itemData);
        }

        //Debug.Log($"Inventory has {GetCount()} instances");
    }

    public void RemoveFromIndex(int index)
    {
        InventoryItem item = inventory[index];
        if (item != null)
        {
            item.RemoveFromStack();
            if (item.stackSize == 0)
            {
                inventory[index] = null;
                itemDictionary.Remove(index);
            }
        }

        inventoryUI.UpdateAll(this);


    }

    public void RemoveFromIndex(int index, int Amount)
    {
        InventoryItem item = inventory[index];
        if (item != null)
        {
            for (int i = 0; i < Amount; i++)
            {
                if (item != null)
                {
                    item.RemoveFromStack();
                    if (item.stackSize == 0)
                    {
                        inventory[index] = null;
                        itemDictionary.Remove(index);
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
           
        }
        inventoryUI.UpdateAll(this);

    }

    public void Remove(ItemData itemData)
    {
        //InventoryItem item = FindItem(itemData);
        //if (item != null)
        //{
        //    item.RemoveFromStack();
        //    if (item.stackSize == 0)
        //    {
        //        inventory[System.Array.IndexOf(inventory, itemdata)] = null;
        //        //inventory.remove(item);
        //        itemdictionary.remove(itemdata);
        //    }
        //}

        inventoryUI.UpdateAll(this);

    }

    public void HandleGunDrop(InventoryItem gunItem)
    {
        Debug.Log($"Attempting to remove weight from index ({gunItem.weightIndex})");
        RemoveFromIndex(gunItem.weightIndex);
    }


    public void DropItem(int index, Transform playerTransform)
    {
        InventoryItem item = inventory[index];
        if (item != null && item.itemData != weightItem)
        {
            if (isItemAGun(item.itemData))
            {
                HandleGunDrop(item);
            }
            float dropDistance = 1.5f;

            Vector3 dropLocation = playerTransform.position + playerTransform.forward * dropDistance;
            Debug.Log($"Attempting to drop item {item.itemData.displayName}");
            
            GameObject.Instantiate(Resources.Load(item.itemData.name), dropLocation, Quaternion.identity);
            RemoveFromIndex(index);

        }
    }

    public void DropItem(int index, Vector3 dropLocation)
    {
        InventoryItem item = inventory[index];
        if (item != null && item.itemData != weightItem)
        {
            if (isItemAGun(item.itemData))
            {
                HandleGunDrop(item);
            }

            Debug.Log($"Attempting to drop item {item.itemData.displayName}");
            GameObject.Instantiate(Resources.Load(item.itemData.name), dropLocation, Quaternion.identity);
            RemoveFromIndex(index);

        }
    }

    public void Consume(int index, GunHolder gunHolder)
    {
        InventoryItem item = inventory[index];
        if (item != null)
        {
            if (isItemAGun(item.itemData))
            {
                // Swap guns
                GunData currentlyEquippedGun = gunHolder.equippedGun;
                gunHolder.equippedGun = item.itemData as GunData;

                if (currentlyEquippedGun != null)
                {
                    // Place currently equipped gun back to inventory
                    Add(currentlyEquippedGun);
                }

                RemoveFromIndex(index);
            }
            else if (item.itemData.consumable == true)
            {
                string itemName = item.itemData.name;

                switch (itemName)
                {
                    case "Medkit":
                        Debug.Log("+100 HP");
                        break;
                    default:
                        Debug.LogWarning($"Consumable item {itemName} is not in the switch list");
                        break;
                }

                RemoveFromIndex(index);
            }
            else
            {
                Debug.Log("Item is not consumable or null");
            }
        }
    }
}
