using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// What goes in the game object to represent the item data
public class InventoryItem 
{

    public ItemData itemData;
    public int stackSize = 1;
    public int weightIndex;

    // On Constructor call set itemData
    public InventoryItem(ItemData itemData)
    {
        this.itemData = itemData;
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

 
}
