using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class InventoryUI 
{
    public int playerIndex;
    public InventoryUI(int playerIndex)
    {
        this.playerIndex = playerIndex;
    }
    
    public void UpdateAll(Inventory inventoryRef)
    {
        Transform InvenUI = GameObject.Find($"Inventory{playerIndex}").transform;
        for (int i = 0; i < inventoryRef.GetCount(); i++)
        {
            // Get objects
            GameObject SlotToUpdate = InvenUI.transform.Find($"Slot{i}").gameObject;
            TextMeshProUGUI StackUI = SlotToUpdate.transform.Find("Stack").gameObject.GetComponent<TextMeshProUGUI>();

            // Get item references
            InventoryItem invenItem = inventoryRef.inventory[i];
            ItemData itemData = invenItem.itemData;

            // Update UI
            SlotToUpdate.GetComponent<Image>().sprite = itemData.icon;
            StackUI.text = invenItem.stackSize.ToString();
       
        }
    }
}
