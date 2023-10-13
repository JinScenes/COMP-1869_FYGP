using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Inventory
{

    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private InventoryUI inventoryUI = new InventoryUI(0);

    public void Add(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            if (item.itemData.canStack)
            {
                item.AddToStack();
                //Debug.Log($"{itemData.displayName} total stack is now {item.stackSize}");
                inventoryUI.UpdateAll(this);
            }
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
            //Debug.Log($"{itemData.displayName} has now been added to the inventory");
            inventoryUI.UpdateAll(this);

        }
    }

    public void Remove(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if(item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }
}
